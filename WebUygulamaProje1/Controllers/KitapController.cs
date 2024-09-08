﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{

    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;

        }
        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
        {

            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps: "KitapTuru").ToList();
            return View(objKitapList);
        }
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.Ad,
                    Value = k.Id.ToString()
                });
            ViewBag.KitapTuruList = KitapTuruList;
            if (id == 0 || id == null)
                return View();
            else
            {
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);

                if (kitapVt == null)
                    return NotFound();

                return View(kitapVt);
            }
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]

        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors); //debug yöntemi

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");

                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        kitap.ResimUrl = @"\img\" + file.FileName;
                    }

                }


                if (kitap.Id == 0)
                {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni kitap eklendi";
                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Kitap güncellendi";
                }


                _kitapRepository.Kaydet();
                return RedirectToAction("Index", "Kitap");
            }
            else
                return View();
        }

        //public IActionResult Guncelle(int? id)
        //{
        //    if (id == 0 || id == null)
        //        return NotFound();

        //    Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); // (Expression<Func<T, bool>> filtre)

        //    if (kitapVt == null)
        //        return NotFound();

        //    return View(kitapVt);
        //}


        //[HttpPost] 
        //public IActionResult Guncelle(Kitap kitap)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _kitapRepository.Guncelle(kitap);
        //        _kitapRepository.Kaydet();
        //        TempData["basarili"] = "Kitap güncellendi";
        //        return RedirectToAction("Index", "Kitap");
        //    }
        //    return View();
        //}


        [Authorize(Roles = UserRoles.Role_Admin)]

        public IActionResult Sil(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);

            if (kitapVt == null)
                return NotFound();

            else
                return View(kitapVt);
        }
        [HttpPost, ActionName("Sil")]

        [Authorize(Roles = UserRoles.Role_Admin)]

        public IActionResult SilPost(int? id)
        {
            Kitap? kitap = _kitapRepository.Get(u => u.Id == id);
            if (kitap == null)
                return NotFound();
            else
            {
                _kitapRepository.Sil(kitap);
                _kitapRepository.Kaydet();
                TempData["basarili"] = "Kitap türü silindi";
                return RedirectToAction("Index", "Kitap");
            }
        }




    }
}
