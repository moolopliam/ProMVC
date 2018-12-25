using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Khruphanth.Models;
using QRCoder;
using System.Drawing;
using System.Data.Entity;
using System.Net;

namespace Khruphanth.Controllers
{
    public class RequestListController : Controller
    {
        // GET: RequestList
        private readonly ComCSDBEntities db = new ComCSDBEntities();
        List<PhatQR> phatQRs = new List<PhatQR>();
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: RequestList/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Waiting", "Requisition");
            }
            var data = db.T_RequestList.Where(x => x.RequestLsitID == id).FirstOrDefault();
            return View(data);
        }

        // GET: RequestList/Create
        public ActionResult Create(string RequestLsitID)
        {
            if (RequestLsitID != null)
            {
                Session["number"] = RequestLsitID;
            }
            List<T_Type> LstType = new List<T_Type>();
            //LstType.Insert(0, new T_Type { TypeID = "0", TY_NameType = "----เลือกชนิด----" });

            List<T_Category> ListCategory = db.T_Category.ToList();
            //ListCategory.Insert(0, new T_Category { CategoryID = "0", CA_NameCategory = "-กรุณาเลือกหมวด-" });

            ViewBag.RL_CategoryID = new SelectList(ListCategory, "CategoryID", "CA_NameCategory");
            ViewBag.RL_TypeID = new SelectList(LstType, "TypeID", "TY_NameType");
            ViewBag.RL_PlaceID = new SelectList(db.T_Place, "PlaceID", "PL_NamePlace");
            return View();
        }

        // POST: RequestList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_RequestList data)
        {
            ViewBag.RL_CategoryID = new SelectList(db.T_Category, "CategoryID", "CA_NameCategory");
            ViewBag.RL_TypeID = new SelectList(db.T_Type, "TypeID", "TY_NameType");
            ViewBag.RL_PlaceID = new SelectList(db.T_Place, "PlaceID", "PL_NamePlace");
            var chk = db.T_Requisition.Where(x => x.RequisitionID == data.RL_RequisitionID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (chk != null)
                {

                    if (data.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageUpload.FileName);
                        string extension = Path.GetExtension(data.ImageUpload.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        data.RL_PictureKhru = "~/Images/" + fileName;
                        data.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
                    }
                    var _Khruphanths = new List<T_Khruphanth>();
                    DateTime date = new DateTime();
                    string str = date.Year.ToString();
                    str = DateTime.Now.ToString("yy");

                    var TmpCODE = "";
                    for (int i = 0; i < data.RL_Amount; i++)
                    {
                        TmpCODE = data.RL_CategoryID.ToString() + "." + data.RL_TypeID + "." + (data.RL_OnStart + i).ToString() + "/" + str;
                    }

                    var Chk_LISKID = db.T_RequestList.ToList();
                    int NumChk = 0; int tmpNum = 0;
                    foreach (var item in Chk_LISKID)
                    {
                        for (int i = 0; i < item.RL_Amount; i++)
                        {

                            NumChk = Convert.ToInt32(item.RL_OnStart + i);

                        }
                    }
                    for (int i = 0; i < data.RL_Amount; i++)
                    {
                        tmpNum = Convert.ToInt32(data.RL_OnStart + i);

                    }

                    var CK_IDKP = db.T_Khruphanth.Where(a => a.KhruphanthID == TmpCODE).ToList();
                    if (CK_IDKP.Count != 0)
                    {
                        ModelState.AddModelError("RL_OnStart", "มีเลขครุภัณฑ์อยู่ในฐานข้อมูลแล้ว กรุณาตรวจสอบอีกครั้ง");
                    }
                    else if (tmpNum == NumChk)
                    {
                        ModelState.AddModelError("RL_OnStart", "เลขครุภัณฑ์ซ้ำในการเบิก กรุณาตรวจสอบอีกครั้ง");
                    }
                    else
                    {

                        db.T_RequestList.Add(data);
                        db.SaveChanges();
                        Session["Result"] = "okC";
                        return RedirectToAction("Details", "Requisition", new { RequisitionID = data.RL_RequisitionID });
                    }

                }
                else
                {
                    ModelState.AddModelError("RL_RequisitionID", "เลขที่ใบเบิกผิด กรุณาตรวจสอบอีกครั้ง");
                }

            }

            return View(data);
        }

        public ActionResult t_Khruphanths(string IDRL)
        {
            var AsData = db.T_RequestList.Where(x => x.RL_RequisitionID == IDRL).ToList();
            var _data = new List<T_RequestList>(AsData);

            if (IDRL != null)
            {

                var chk = db.T_Requisition.Where(x => x.RequisitionID == IDRL).FirstOrDefault();
                var chk1 = db.T_Requisition.Where(x => x.RequisitionID == IDRL).ToList();
                if (chk1.Count > 0)
                {
                    //chk.Re_StepID = "1";

                    var _Khruphanths = new List<T_Khruphanth>();
                    DateTime date = new DateTime();
                    string str = date.Year.ToString();
                    str = DateTime.Now.ToString("yy");

                    foreach (var data in _data)
                    {
                        var TmpCODE = "";
                        for (int i = 0; i < data.RL_Amount; i++)
                        {

                            TmpCODE = data.RL_CategoryID.ToString() + "." + data.RL_TypeID + "." + (data.RL_OnStart + i).ToString() + "/" + str;
                            QRCode qRCode = new QRCode();
                            string code = TmpCODE;
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            //QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                            imgBarCode.Height = 250;
                            imgBarCode.Width = 250;
                            byte[] byteImage = null;
                            QRCode xx = new QRCode(qrCodeData);
                            using (Bitmap bitMap = xx.GetGraphic(20))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                   byteImage = ms.ToArray();
                                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                                    string result = Convert.ToBase64String(byteImage, 0, byteImage.Length); ;                                 
                                    //CreateImage(result);
                                }
                                //plQRCode.Controls.Add(imgBarCode);

                            }
                            //foreach (var item in phatQRs)
                            //{

                            //    Test = item.Path;
                            //}

                            _Khruphanths.Add(new T_Khruphanth { KhruphanthID = TmpCODE, Kh_RequestLsitID = data.RequestLsitID, Kh_StatusID = 1, Kh_PlaceID = data.RL_PlaceID, Kh_QR_CODE = byteImage });
                        }
                    }

                    db.T_Khruphanth.AddRange(_Khruphanths);
                    chk.Re_StepID = "1";
                    db.SaveChanges();
                    Mo(chk);
                    Session["Result"] = "okC";
                    return RedirectToAction("Index", "Khruphanths");
                }
                else
                {
                    Session["Result"] = "error0";
                    return RedirectToAction("Details", "Requisition");
                }
            }
            else
            {
                Session["Result"] = "error1";
                return RedirectToAction("Details", "Requisition");

            }


        }


        public void Mo(T_Requisition IDPOST)
        {
            //var AsData = db.T_Requisition.ToList();
            //var _data = new List<T_Requisition>(db.T_Requisition);
            //_data.Where(x => x.RequisitionID == IDPOST).Select(a => a.Re_StepID == "1").FirstOrDefault();
            db.Entry(IDPOST).State = EntityState.Modified;
            db.SaveChanges();
        }


        public string CreateImage(string Byt)
        {

            try
            {
                byte[] data = Convert.FromBase64String(Byt);
                var filename = Convert.ToString(System.Guid.NewGuid()).Substring(0, 5) + Convert.ToString(System.Guid.NewGuid()).Substring(0, 5) + System.DateTime.Now.ToString("FFFFFF") + System.DateTime.Now.Minute.ToString() + ".png";// +System.DateTime.Now.ToString("fffffffffff") + ".png";
                var file = HttpContext.Server.MapPath("~/QR_CODE/" + filename);
                var Path = "~/QR_CODE/" + filename;
                phatQRs.Add(new PhatQR { Path = Path });
                System.IO.File.WriteAllBytes(file, data);
                string ImgName = ".../profileimages/" + filename;

                return filename;

            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        // GET: RequestList/Edit/5
        public ActionResult Edit(int id)
        {
            //List<T_Type> LstType = new List<T_Type>();
            ////LstType.Insert(0, new T_Type { TypeID = "0", TY_NameType = "----เลือกชนิด----" });
            ModelState.Remove("ImageUpload");
            var data = db.T_RequestList.Where(a => a.RequestLsitID == id).FirstOrDefault();
            //List<T_Category> ListCategory = db.T_Category.ToList();
            //ListCategory.Insert(0, new T_Category { CategoryID = "0", CA_NameCategory = "-กรุณาเลือกหมวด-" });
            TempData["ID"] = data.RL_RequisitionID;

            ViewBag.RL_CategoryID = new SelectList(db.T_Category, "CategoryID", "CA_NameCategory", data.RL_CategoryID);
            ViewBag.RL_TypeID = new SelectList(db.T_Type, "TypeID", "TY_NameType", data.RL_TypeID);
            ViewBag.RL_PlaceID = new SelectList(db.T_Place, "PlaceID", "PL_NamePlace", data.RL_PlaceID);

            return View(data);
        }

        // POST: RequestList/Edit/5
        [HttpPost]
        public ActionResult Edit(T_RequestList data)
        {
            ViewBag.RL_CategoryID = new SelectList(db.T_Category, "CategoryID", "CA_NameCategory", data.RL_CategoryID);
            ViewBag.RL_TypeID = new SelectList(db.T_Type, "TypeID", "TY_NameType", data.RL_TypeID);
            ViewBag.RL_PlaceID = new SelectList(db.T_Place, "PlaceID", "PL_NamePlace", data.RL_PlaceID);

            if (data.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(data.ImageUpload.FileName);
                string extension = Path.GetExtension(data.ImageUpload.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                data.RL_PictureKhru = "~/Images/" + fileName;
                data.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
            }
            if (ModelState.IsValid)
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                Session["Result"] = "okE";
                return RedirectToAction("Details", "Requisition", new { RequisitionID = data.RL_RequisitionID });
            }
            return View(data);
        }

        public ActionResult Delete(int id)
        {

                var data = db.T_RequestList.Where(a => a.RequestLsitID == id).FirstOrDefault();
                db.T_RequestList.Remove(data);
                db.SaveChanges();
                Session["Result"] = "ok";
                return RedirectToAction("Details", "Requisition", new { RequisitionID = data.RL_RequisitionID });
            
        }
    }
}
