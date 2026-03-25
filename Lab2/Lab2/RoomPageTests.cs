using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

/* Модуль: Комната совместного просмотра */
namespace VmesteFilmsTests
{
    public class RoomPageTests : TestBase
    {
        private string baseUrl = "https://x.vmestefilms.online/";

        [SetUp]
        public void OpenRoomPage()
        {
            driver.Navigate().GoToUrl(baseUrl);

            LoginTestUser();
            GoToMain();
            GoToVideoPage();
            GoToRoomPage();
        }

        /*
            Тест 1: Проверка копирования ссылки приглашения
            Ожидание: Ссылка на ккомнату копируется в буфер обмена
            Примечание: У Selenium нельзя напрямую прверить буфер обмена, поэтому просто проверяем, 
                        что кнопка кликабельна и не вызывает ошибок при нажатии
        */
        [Test]
        public void Test1_CopyInviteLink()
        {
            var copyBtn = driver.FindElements(By.Id("copy-to-clipboard")).FirstOrDefault();
            Assert.IsNotNull(copyBtn, "Кнопка копирования ссылки не найдена");

            copyBtn.Click();

            Assert.Pass("Кнопка копирования нажата");
        }

        /*
            Тест 2: Проверка ввода и отправки сообщения в чате комнаты
            Ожидание: Пользователь может ввести и отправить текст сообщения
        */
        [Test]
        public void Test2_EnterChatMessage()
        {
            var messageInput = driver.FindElements(By.XPath("//*[@id=\"send-message-form\"]/div[2]")).FirstOrDefault();
            Assert.IsNotNull(messageInput, "Поле ввода сообщения не найдено");

            string messageText = $"Тестовое сообщение {DateTime.Now.Ticks}";

            messageInput.Click();
            messageInput.Clear();
            messageInput.SendKeys(messageText);
            messageInput.SendKeys(Keys.Enter);

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d =>
                {
                    var input = d.FindElements(By.XPath("//*[@id=\"chat\"]/div[2]/div[2]/div[2]"));
                    return input.Any(m => m.Text.Trim() == messageText);
                });

            var sentMessage = driver.FindElements(By.XPath("//*[@id=\"chat\"]/div[2]/div[2]/div[2]"))
                                    .FirstOrDefault(m => m.Text.Trim() == messageText);
            Assert.IsNotNull(sentMessage, "Сообщение не появилось в чате после отправки");
        }

        /*
            Тест 3: Проверка выхода из комнаты на главную страницу
            Ожидание: Пользователь возвращается на главную страницу
        */
        [Test]
        public void Test3_ExitToMainPage()
        {
            var homeBtn = driver.FindElements(By.XPath("//*[@id=\"chats\"]/div[2]/div[1]/a[1]/img")).FirstOrDefault();
            Assert.IsNotNull(homeBtn, "Кнопка 'Домой' не найдена");

            homeBtn.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url == baseUrl);

            Assert.AreEqual(baseUrl, driver.Url, "Переход на главную страницу не произошел");
        }
    }
}