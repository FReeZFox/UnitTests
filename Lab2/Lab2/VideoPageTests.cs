using NUnit.Framework;
using OpenQA.Selenium;

/* Модуль: Страница видео-файла */
namespace VmesteFilmsTests
{
    public class VideoPageTests : TestBase
    {
        private string baseUrl = "https://x.vmestefilms.online/";

        [SetUp]
        public void OpenMainPage()
        {
            driver.Navigate().GoToUrl(baseUrl);

            LoginTestUser();
            GoToMain();
            GoToVideoPage();
        }

        /*
            Тест 1: Проверка создания комнаты для совместного просмотра
            Ожидание: Пользователь переходит в комнату просмотра после нажатия "Смотреть"
        */
        [Test]
        public void Test1_CreateWatchRoom()
        {
            GoToRoomPage();
        }

        /*
            Тест 2: Проверка написания комментария
            Ожидание: Пользователь успешно публикует комментарий
            Примечание: Тест был слегка изменен. Ранее предполагалось, что комменетарий нужно опубликовать, 
                        но чтобы не засорять страницу видео бессмысленными комментариями, проверялся только сам ввод комментария без отправки
        */
        [Test]
        public void Test2_EnterComment()
        {
            var commentInput = driver.FindElements(By.Name("comment")).FirstOrDefault();
            Assert.IsNotNull(commentInput, "Поле ввода комментария не найдено");

            string commentText = $"Тестовый комментарий {DateTime.Now.Ticks}";

            commentInput.Click();
            commentInput.Clear();
            commentInput.SendKeys(commentText);

            Assert.AreEqual(commentText, commentInput.GetAttribute("value"),
                "Текст комментария не был введен в поле");
        }
    }
}