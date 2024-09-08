using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KiralamaController(IKiralamaRepository kiralamaRepository,
                                  IKitapRepository kitapRepository,
                                  IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps: "Kitap").ToList();
            return View(objKiralamaList);
        }
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.KitapAdi,
                    Value = k.Id.ToString()
                });
            ViewBag.KitapList = KitapList;

            if (id == 0 || id == null)
                return View();
            else
            {
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);

                if (kiralamaVt == null)
                    return NotFound();

                return View(kiralamaVt);
            }
        }
        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
            if (ModelState.IsValid)
            {
                if (kiralama.Id == 0)
                {
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Yeni Kiralama eklendi";
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "Kiralama güncellendi";
                }


                _kiralamaRepository.Kaydet();
                return RedirectToAction("Index", "Kiralama");
            }
            else
                return View();
        }

        public IActionResult Sil(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.KitapAdi,
                    Value = k.Id.ToString()
                });
            ViewBag.KitapList = KitapList;

            if (id == 0 || id == null)
                return NotFound();

            Kiralama? KiralamaVt = _kiralamaRepository.Get(u => u.Id == id);

            if (KiralamaVt == null)
                return NotFound();

            else
                return View(KiralamaVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPost(int? id)
        {
            Kiralama Kiralama = _kiralamaRepository.Get(u => u.Id == id);
            if (Kiralama == null)
                return NotFound();
            else
            {
                _kiralamaRepository.Sil(Kiralama);
                _kiralamaRepository.Kaydet();
                TempData["basarili"] = "Kiralama silindi";
                return RedirectToAction("Index", "Kiralama");
            }
        }
    }
}
