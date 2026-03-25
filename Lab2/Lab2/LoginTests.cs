using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

/* Модуль: Страница авторизации */
namespace VmesteFilmsTests
{
    public class LoginTests : TestBase
    {
        private string baseUrl = "https://x.vmestefilms.online/";

        [SetUp]
        public void OpenMainPage()
        {
            driver.Navigate().GoToUrl(baseUrl);

            GoToLogin();
        }

        /*
            Тест 1: Проверка наличия и кликабельности кнопки "Вход" на главной странице
            Ожидание: Кнопка "Вход" присутствует доступна для клика на главной странице
        */
        [Test]
        public void Test1_LoginButtonClickable()
        {
            var loginBtn = driver.FindElements(By.LinkText("Вход")).FirstOrDefault();
            Assert.IsNotNull(loginBtn);
            Assert.IsTrue(loginBtn.Displayed);
            Assert.IsTrue(loginBtn.Enabled);
        }

        /*
            Тест 2: Проверка перехода на страницу регистрации при клике на кнопку "Вход"
            Ожидание: При клике на кнопку "Вход" пользователь перенаправляется на страницу авторизации
                      (URL содержит "login")
        */
        [Test]
        public void Test2_LoginButton_Click()
        {
            Assert.IsTrue(driver.Url.Contains("login"), "Перехода не произошло");
        }

        /*
            Тест 3: Проверка наличия всех необходимых элементов на странице авторизации
                    (email, пароля, кнопки авторизации, ссылки на Telegram, регистрацию и восстановление пароля)
            Ожидание: На странице авторизации присутствуют все необходимые элементы 
        */
        [Test]
        public void Test3_LoginForm()
        {
            Assert.IsTrue(driver.FindElements(By.Name("username")).Count > 0, "Нет поля email");
            Assert.IsTrue(driver.FindElements(By.Name("password")).Count > 0, "Нет поля пароля");
            Assert.IsTrue(driver.FindElements(By.XPath("//button[contains(text(),'Вход')]")).Count > 0, "Нет кнопки авторизации");
            Assert.IsTrue(driver.PageSource.Contains("Telegram"), "Нет кнопки Telegram");
            Assert.IsTrue(driver.PageSource.Contains("Зарегистрироваться"), "Нет ссылки регистрации");
            Assert.IsTrue(driver.PageSource.Contains("Восстановить"), "Нет ссылки восстановления пароля");
        }

        /*
            Тест 4: Проверка авторизации с валидными данными
            Ожидание: При вводе валидных данных и клике на кнопку "Вход" пользователь успешно авторизуется
                      и перенаправляется на главную страницу (URL главной страницы  не содержит "login")
        */
        [Test]
        public void Test4_Login_WithValidData()
        {
            driver.FindElement(By.Name("username")).SendKeys("testuser123@mail");
            driver.FindElement(By.Name("password")).SendKeys("Very_123456Strong!Password");
            driver.FindElement(By.XPath("//button[contains(text(),'Вход')]")).Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url == baseUrl || !d.Url.Contains("login"));

            Assert.IsFalse(driver.Url.Contains("login"), "Авторизация не удалась");
        }

        /*
            Тест 5: Проверка авторизации без ввода email
            Ожидание: При попытке авторизоваться без указания email отображается сообщение об ошибке 
                      "Заполните это поле" и пользователь остается на странице авторизации (URL содержит "login")
        */
        [Test]
        public void Test5_Login_WithoutEmail()
        {
            driver.FindElement(By.Name("password")).SendKeys("Very_123456Strong!Password");
            driver.FindElement(By.XPath("//button[contains(text(),'Вход')]")).Click();

            Assert.IsTrue(driver.Url.Contains("login"), "Авторизация прошла без email");
        }

        /*
            Тест 6: Проверка авторизации с невалидным email
            Ожидание: При попытке авторизоваться с невалидным email отображается сообщение об ошибке 
                      "Неправильное имя пользователя или пароль" и пользователь остается на странице авторизации (URL содержит "login")
        */
        [Test]
        public void Test6_Login_InvalidEmail()
        {
            driver.FindElement(By.Name("username")).SendKeys("invalid-email");
            driver.FindElement(By.Name("password")).SendKeys("Very_123456Strong!Password");
            driver.FindElement(By.XPath("//button[contains(text(),'Вход')]")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Неправильное имя пользователя или пароль"), "Сообщения об ошибке не появилось");
            Assert.IsTrue(driver.Url.Contains("login"), "Авторизация прошла с невалидным email");
        }

        /*
            Тест 7: Проверка авторизации без ввода пароля
            Ожидание: При попытке авторизоваться без указания пароля отображается сообщение об ошибке 
                      "Заполните это поле" и пользователь остается на странице авторизации (URL содержит "login")
        */
        [Test]
        public void Test7_Login_WithoutPassword()
        {
            driver.FindElement(By.Name("username")).SendKeys("testuser123@mail");
            driver.FindElement(By.XPath("//button[contains(text(),'Вход')]")).Click();

            Assert.IsTrue(driver.Url.Contains("login"), "Авторизация прошла без пароля");
        }

        /*
            Тест 8: Проверка авторизации с неверным паролем 
            Ожидание: При попытке авторизоваться с неверным паролем отображается сообщение об ошибке 
                      "Неправильное имя пользователя или пароль" и пользователь остается на странице авторизации (URL содержит "login")
        */
        [Test]
        public void Test8_Login_InvalidPassword()
        {
            driver.FindElement(By.Name("username")).SendKeys("testuser123@mail");
            driver.FindElement(By.Name("password")).SendKeys("incorrect-password");
            driver.FindElement(By.XPath("//button[contains(text(),'Вход')]")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Неправильное имя пользователя или пароль"), "Сообщения об ошибке не появилось");
            Assert.IsTrue(driver.Url.Contains("login"), "Авторизация прошла с неверным паролем");
        }
    }
}