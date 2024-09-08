using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KitapTuruController : Controller
    {
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapTuruRepository = context;
        }
        public IActionResult Index()
        {
            List<KitapTuru> objKitapTuruList = _kitapTuruRepository.GetAll().ToList();
            return View(objKitapTuruList);
        }
        public IActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid)
            {
                _kitapTuruRepository.Ekle(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Yeni kitap türü eklendi";
                return RedirectToAction("Index", "KitapTuru");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Guncelle(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == id); // (Expression<Func<T, bool>> filtre)

            if (kitapTuruVt == null)
                return NotFound();

            return View(kitapTuruVt);
        }
        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid)
            {
                _kitapTuruRepository.Guncelle(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Kitap türü güncellendi";
                return RedirectToAction("Index", "KitapTuru");
            }
            return View();
        }

        public IActionResult Sil(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == id);

            if (kitapTuruVt == null)
                return NotFound();

            else
                return View(kitapTuruVt);
        }
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPost(int? id)
        {
            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == id);
            if (kitapTuru == null)
                return NotFound();
            else
            {
                _kitapTuruRepository.Sil(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Kitap türü silindi";
                return RedirectToAction("Index", "KitapTuru");
            }
        }




    }
}
