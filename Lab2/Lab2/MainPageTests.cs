using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

/* Модуль: Главная страница */
namespace VmesteFilmsTests
{
    public class MainPageTests : TestBase
    {
        private string baseUrl = "https://x.vmestefilms.online/";

        [SetUp]
        public void OpenMainPage()
        {
            driver.Navigate().GoToUrl(baseUrl);

            LoginTestUser();
            GoToMain();
        }

        /*
            Тест 1: Проверка пооиска видео-файлов
            Ожидание: Пользователь успешно выполняет поиск и переходит на страницу результатов
        */
        [Test]
        public void Test1_SearchVideo()
        {
            var searchInput = driver.FindElements(By.XPath("//*[@id='inputSearch']")).FirstOrDefault();
            Assert.IsNotNull(searchInput, "Поле поиска не найдено");

            string query = "фильм";

            searchInput.Click();
            searchInput.SendKeys(query);
            searchInput.SendKeys(Keys.Enter);

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("search"));

            Assert.IsTrue(driver.Url.Contains("search"), "Переход на страницу результатов поиска не произошел");
        }

        /*
            Тест 2: Проверка выбора видео-файла по жанру
            Ожидание: Пользователь переходит на страницу с подборкой по выбранному жанру
        */
        [Test]
        public void Test2_SelectGenre()
        {
            var genresDropdown = driver.FindElements(By.LinkText("Жанры")).FirstOrDefault();
            Assert.IsNotNull(genresDropdown, "Список жанров не найден");
            genresDropdown.Click();

            var genreItem = driver.FindElements(By.XPath("//*[@id='navbarNav']/ul[1]/li[1]/div/a[10]")).FirstOrDefault();
            Assert.IsNotNull(genreItem, "Жанр не найден");
            genreItem.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("genre"));

            Assert.IsTrue(driver.Url.Contains("genre"), "Переход на страницу жанра не произошел");
        }

        /*
            Тест 3: Просмотр страницы видео-файла
            Ожидание: Пользователь переходит на страницу выбранного видео-файла
        */
        [Test]
        public void Test3_OpenVideoPage()
        {
            GoToVideoPage();
        }
    }
}