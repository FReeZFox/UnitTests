using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

/* Модуль: Страница регистрации */ 
namespace VmesteFilmsTests
{
    public class RegistrationTests : TestBase
    {
        private string baseUrl = "https://x.vmestefilms.online/";

        [SetUp]
        public void OpenMainPage()
        {
            driver.Navigate().GoToUrl(baseUrl);

            GoToRegistration();
        }

        /*
            Тест 1: Проверка наличия и кликабельности кнопки "Регистрация" на главной странице
            Ожидание: Кнопка "Регистрация" присутствует доступна для клика на главной странице
        */
        [Test]
        public void Test1_RegisterButtonClickable()
        {
            var registerBtn = driver.FindElements(By.LinkText("Регистрация")).FirstOrDefault();
            Assert.IsNotNull(registerBtn);
            Assert.IsTrue(registerBtn.Displayed);
            Assert.IsTrue(registerBtn.Enabled);
        }

        /*
            Тест 2: Проверка перехода на страницу регистрации при клике на кнопку "Регистрация"
            Ожидание: При клике на кнопку "Регистрация" пользователь перенаправляется на страницу регистрации 
                      (URL содержит "registration")
        */
        [Test]
        public void Test2_RegisterButton_Click()
        {
            Assert.IsTrue(driver.Url.Contains("registration"), "Перехода не произошло");
        }

        /*
            Тест 3: Проверка наличия всех необходимых элементов на странице регистрации 
                    (поля для имени, email, пароля, кнопки регистрации, ссылки на Telegram, вход и правила)
            Ожидание: На странице регистрации присутствуют все необходимые элементы 
        */
        [Test]
        public void Test3_RegistrationForm()
        {
            Assert.IsTrue(driver.FindElements(By.Name("name_register")).Count > 0, "Нет поля имени");
            Assert.IsTrue(driver.FindElements(By.Name("username_register")).Count > 0, "Нет поля email");
            Assert.IsTrue(driver.FindElements(By.Name("password_register")).Count > 0, "Нет поля пароля");
            Assert.IsTrue(driver.FindElements(By.XPath("//button[contains(text(),'Зарегистрироваться')]")).Count > 0, "Нет кнопки регистрации");
            Assert.IsTrue(driver.PageSource.Contains("Telegram"), "Нет кнопки Telegram");
            Assert.IsTrue(driver.PageSource.Contains("Войти"), "Нет ссылки входа");
            Assert.IsTrue(driver.PageSource.Contains("Правила"), "Нет ссылки правил");
        }

        /*
            Тест 4: Проверка регистрации с валидными данными
            Ожидание: При вводе валидных данных и клике на кнопку "Зарегистрироваться" пользователь успешно регистрируется 
                      и перенаправляется на главную страницу (URL главной страницы  не содержит "registration")
        */
        [Test]
        public void Test4_Registration_WithValidData()
        {
            string uniqueEmail = $"test{DateTime.Now.Ticks}@mail.com";

            driver.FindElement(By.Name("name_register")).SendKeys("testuser");
            driver.FindElement(By.Name("username_register")).SendKeys(uniqueEmail);
            driver.FindElement(By.Name("password_register")).SendKeys("Test123!");
            driver.FindElement(By.XPath("//button[contains(text(),'Зарегистрироваться')]")).Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url == baseUrl || !d.Url.Contains("registration"));

            Assert.IsFalse(driver.Url.Contains("registration"), "Регистрация не удалась");
        }

        /*
            Тест 5: Проверка регистрации без ввода email
            Ожидание: При попытке зарегистрироваться без указания email отображается сообщение об ошибке 
                      "Неправильно указан e-mail" и пользователь остается на странице регистрации (URL содержит "registration")
        */
        [Test]
        public void Test5_Registration_WithoutEmail()
        {
            driver.FindElement(By.Name("name_register")).SendKeys("testuser");
            driver.FindElement(By.Name("password_register")).SendKeys("Test123!");
            driver.FindElement(By.XPath("//button[contains(text(),'Зарегистрироваться')]")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Неправильно указан e-mail"), "Сообщения об ошибке не появилось");
            Assert.IsTrue(driver.Url.Contains("registration"), "Регистрация прошла без email");
        }

        /*
            Тест 6: Проверка регистрации с невалидным email
            Ожидание: При попытке зарегистрироваться с невалидным email отображается сообщение об ошибке 
                      "Неправильно указан e-mail" и пользователь остается на странице регистрации (URL содержит "registration")
        */
        [Test]
        public void Test6_Registration_InvalidEmail()
        {
            driver.FindElement(By.Name("name_register")).SendKeys("testuser");
            driver.FindElement(By.Name("username_register")).SendKeys("invalid-email");
            driver.FindElement(By.Name("password_register")).SendKeys("Test123!");
            driver.FindElement(By.XPath("//button[contains(text(),'Зарегистрироваться')]")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Неправильно указан e-mail"), "Сообщения об ошибке не появилось");
            Assert.IsTrue(driver.Url.Contains("registration"), "Регистрация прошла с невалидным email");
        }

        /*
            Тест 7: Проверка регистрации без ввода пароля
            Ожидание: При попытке зарегистрироваться без указания пароля отображается сообщение об ошибке 
                      "Неправильно указан пароль" и пользователь остается на странице регистрации (URL содержит "registration")
        */
        [Test]
        public void Test7_Registration_WithoutPassword()
        {
            driver.FindElement(By.Name("name_register")).SendKeys("testuser");
            driver.FindElement(By.Name("username_register")).SendKeys("test@mail.com");
            driver.FindElement(By.XPath("//button[contains(text(),'Зарегистрироваться')]")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Неправильно указан пароль"), "Сообщения об ошибке не появилось");
            Assert.IsTrue(driver.Url.Contains("registration"), "Регистрация прошла без пароля");
        }

        /*
            Тест 8: Проверка регистрации с невалидным паролем (очень слабый, небезопасный)
            Ожидание: При попытке зарегистрироваться с невалидным паролем отображается сообщение об ошибке 
                      "Неправильно указан пароль" и пользователь остается на странице регистрации (URL содержит "registration")
        */
        [Test]
        public void Test8_Registration_InvalidPassword()
        {
            string uniqueEmail = $"test{DateTime.Now.Ticks}@mail.com";

            driver.FindElement(By.Name("name_register")).SendKeys("testuser");
            driver.FindElement(By.Name("username_register")).SendKeys(uniqueEmail);
            driver.FindElement(By.Name("password_register")).SendKeys("1"); 
            driver.FindElement(By.XPath("//button[contains(text(),'Зарегистрироваться')]")).Click();

            Assert.IsTrue(driver.PageSource.Contains("Неправильно указан пароль"), "Сообщения об ошибке не появилось");
            Assert.IsTrue(driver.Url.Contains("registration"), "Регистрация прошла с невалидным паролем");
        }

        /*
            Тест 9: Проверка наличия и кликабельности ссылки "Правила" на странице регистрации 
            Ожидание: Ссылка "Правила" присутствует и кликабельна на странице регистрации, 
                      при клике пользователь перенаправляется на страницу с правилами (URL не содержит "registration") 
        */
        [Test]
        public void Test9_TermsLink()
        {
            var link = driver.FindElements(By.PartialLinkText("Правила использования")).FirstOrDefault();
            Assert.IsNotNull(link);
            link.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => !d.Url.Contains("registration"));

            Assert.IsFalse(driver.Url.Contains("registration"), "Страница правил использованя не открыта");
        }
    }
}