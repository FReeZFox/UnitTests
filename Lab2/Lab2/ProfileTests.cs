using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

/* Модуль: Личный кабинет */
namespace VmesteFilmsTests
{
    public class ProfileTests : TestBase
    {
        private string baseUrl = "https://x.vmestefilms.online/";

        [SetUp]
        public void OpenMainPage()
        {
            driver.Navigate().GoToUrl(baseUrl);

            LoginTestUser();
            GoToSettings();
        }

        /*
            Тест 1: Проверка выхода из аккаунта
            Ожидание: Пользователь успешно выходит из учетной записи через личный кабинет
        */
        [Test]
        public void Test1_Logout()
        {
            var logoutLink = driver.FindElements(By.LinkText("выход")).FirstOrDefault();
            Assert.IsNotNull(logoutLink, "Ссылка выхода не найдена");
            logoutLink.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url == baseUrl || !d.Url.Contains("profile"));

            Assert.IsFalse(driver.Url.Contains("profile"), "Выход из аккаунта не произошел");
        }

        /*
            Тест 2: Проверка смены имени аккаунта
            Ожидание: Пользователь успешно меняет имя аккаунта в настройках профиля
        */
        [Test]
        public void Test2_ChangeProfileName()
        {
            var nameInput = driver.FindElements(By.Name("name")).FirstOrDefault();
            Assert.IsNotNull(nameInput, "Поле ввода имени не найдено");

            string uniqueName = $"{DateTime.Now.Ticks}";

            nameInput.Click();
            nameInput.Clear();
            nameInput.SendKeys(uniqueName);
            nameInput.SendKeys(Keys.Enter);

            new WebDriverWait(driver, TimeSpan.FromSeconds(15))
                .Until(d =>
                {
                    try
                    {
                        var input = d.FindElements(By.Name("name")).FirstOrDefault();
                        return input != null && input.GetAttribute("value") == uniqueName;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                });

            // После сохранения изменений, в приложении происходит выход из меню настроек
            GoToSettings();

            var finalInput = driver.FindElements(By.Name("name")).FirstOrDefault();
            Assert.IsNotNull(finalInput, "Поле ввода имени не найдено после обновления");
            Assert.AreEqual(uniqueName, finalInput.GetAttribute("value"), "Имя профиля не изменилось");
        }

        /*
            Тест 3: Проверка смены пароля аккаунта
            Ожидание: Пользователь успешно меняет пароль в настройках профиля. После смены пароля выполняется 
            автоматический выход из аккаунта, вход с новым паролем является ожидаемым результатом теста.
        */
        [Test]
        public void Test3_ChangeProfilePassword()
        {
            string oldPassword = "Very_123456Strong!Password";
            string newPassword = "VeryVery_123456789Strong!Password";

            var passwordInput = driver.FindElements(By.Name("password")).FirstOrDefault();
            Assert.IsNotNull(passwordInput, "Поле ввода пароля не найдено");

            passwordInput.Click();
            passwordInput.Clear();
            passwordInput.SendKeys(newPassword);
            passwordInput.SendKeys(Keys.Enter);

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("login") || d.Url == baseUrl);

            driver.FindElement(By.LinkText("Вход")).Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("login"));

            driver.FindElement(By.Name("username")).SendKeys("testuser123@mail");
            driver.FindElement(By.Name("password")).SendKeys(newPassword);
            driver.FindElement(By.XPath("//button[contains(text(),'Вход')]")).Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => !d.Url.Contains("login"));

            Assert.IsFalse(driver.Url.Contains("login"), "Не удалось войти с новым паролем");
        }
    }
}