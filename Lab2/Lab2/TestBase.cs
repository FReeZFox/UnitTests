using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace VmesteFilmsTests
{
    public abstract class TestBase
    {
        protected IWebDriver driver;
        private string baseUrl = "https://x.vmestefilms.online/";

        [SetUp]
        public void StartBrowser()
        {
            var driverDir = @"D:\3rdparty\chrome";
            var exePath = Path.Combine(driverDir, "chromedriver.exe");

            if (File.Exists(exePath))
            {
                driver = new ChromeDriver(driverDir);
            }
            else
            {
                try
                {
                    driver = new ChromeDriver();
                }
                catch (Exception ex)
                {
                    Assert.Fail($"ChromeDriver not found at '{exePath}' and automatic start failed: {ex.Message}");
                }
            }
        }

        [TearDown]
        public void CloseBrowser()
        {
            try
            {
                driver?.Quit();
            }
            catch { }
        }

        /*
            Вспомогательная функция: выполняет переход на страницу регистрации, 
            кликая на кнопку "Регистрация" на главной странице
        */
        protected void GoToRegistration()
        {
            var registerBtn = driver.FindElements(By.LinkText("Регистрация")).FirstOrDefault();
            Assert.IsNotNull(registerBtn, "Кнопка регистрации не найдена");
            registerBtn.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("registration"));
        }

        /*
            Вспомогательная функция: выполняет переход на страницу авторизации, 
            кликая на кнопку "Вход" на главной странице
        */
        protected void GoToLogin()
        {
            var loginBtn = driver.FindElements(By.LinkText("Вход")).FirstOrDefault();
            Assert.IsNotNull(loginBtn, "Кнопка авторизации не найдена");
            loginBtn.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("login"));
        }

        /*
            Вспомогательная функция: выполняет переход на страницу авторизации
            и выполняет авторизацию с тестовыми данными пользователя
        */
        protected void LoginTestUser()
        {
            GoToLogin();

            driver.FindElement(By.Name("username")).SendKeys("testuser123@mail");
            driver.FindElement(By.Name("password")).SendKeys("Very_123456Strong!Password");
            driver.FindElement(By.XPath("//button[contains(text(),'Вход')]")).Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => !d.Url.Contains("login"));

            Assert.IsFalse(driver.Url.Contains("login"), "Авторизация не удалась");
        }

        /*
            Вспомогательная функция: выполняет переход на страницу настроек профиля
            через клик по имени пользователя в верхнем меню, а затем по ссылке (ред)
        */
        protected void GoToSettings()
        {
            var userNameLink = driver.FindElements(By.XPath("//a[contains(@class,'username')]")).FirstOrDefault();
            Assert.IsNotNull(userNameLink, "Ссылка имени пользователя");
            userNameLink.Click();

            var editLink = driver.FindElements(By.LinkText("(ред)")).FirstOrDefault();
            Assert.IsNotNull(editLink, "Ссылка редактирования профиля не найдена");
            editLink.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("profile"));
        }

        /*
            Вспомогательная функция: выполняет переход на главную страницу сервиса кликом по логотипу в верхнем меню
        */
        protected void GoToMain()
        {
            var logo = driver.FindElements(By.XPath("/html/body/nav/div/div[1]/a/img")).FirstOrDefault();
            Assert.IsNotNull(logo, "Логотип сайта не найден");
            logo.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url == baseUrl);
        }

        /*
            Вспомогательная функция: выполняет переход на страницу видео-файла,
            кликая на карточку видео (любую) на главной странице
        */
        protected void GoToVideoPage()
        {
            var videoCard = driver.FindElements(By.XPath("//a[contains(@class,'film-preview-a')]")).FirstOrDefault();
            Assert.IsNotNull(videoCard, "Карточка видео не найдена");
            videoCard.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("kino"));

            Assert.IsTrue(driver.Url.Contains("kino"), "Переход на страницу видео не произошел");
        }

        /*
            Вспомогательная функция: выполняет переход к комнате совместного просмотра
        */
        protected void GoToRoomPage()
        {
            var scrollBtn = driver.FindElements(By.XPath("//a[@href='#players']")).FirstOrDefault();
            Assert.IsNotNull(scrollBtn, "Кнопка перехода к плеерам не найдена");
            scrollBtn.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElements(By.XPath("//button[starts-with(@id,'player-')]")).Any());

            var playerButton = driver.FindElements(By.XPath("//button[starts-with(@id,'player-')]")).FirstOrDefault();
            Assert.IsNotNull(playerButton, "Кнопки плееров не найдены");
            playerButton.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("room"));

            Assert.IsTrue(driver.Url.Contains("room"), "Переход в комнату просмотра не произошел");
        }
    }
}
