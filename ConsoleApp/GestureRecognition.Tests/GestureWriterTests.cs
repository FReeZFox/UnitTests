namespace ConsoleApp.Tests
{
    public class GestureWriterTests
    {
        // Предполагается, что эти действия завязаны на кнопки
        /*
            Тест 1: Проверка ручного добавления жеста
        */
        [Fact]
        public void Writer_Test1()
        {
            var writer = new GestureWriter();
            writer.Write("А");
            Assert.Equal("А", writer.GetPhrase());
        }
        
        /*
            Тест 2: Проверка удаления последнего жеста
        */
        [Fact]
        public void Writer_Test2()
        {
            var writer = new GestureWriter();
            writer.Write("А");
            writer.Write("Б");
            writer.Erase();
            Assert.Equal("А", writer.GetPhrase());
        }
        
        /*
            Тест 3: Проверка добавления пробела
        */
        [Fact]
        public void Writer_Test3()
        {
            var writer = new GestureWriter();
            writer.Write("А");
            writer.Space();
            writer.Write("Б");
            Assert.Equal("А Б", writer.GetPhrase());
        }
        
        /*
            Тест 4: Проверка очистки всей фразы
        */
        [Fact]
        public void Writer_Test4()
        {
            var writer = new GestureWriter();
            writer.Write("А");
            writer.Write("Б");
            writer.Clear();
            Assert.Equal("", writer.GetPhrase());
        }
        
        /*
            Тест 5: Проверка игнорирования пустого жеста (не должен записываться)
        */
        [Fact]
        public void Writer_Test5()
        {
            var writer = new GestureWriter();
            writer.Write("no_event");
            Assert.Equal("", writer.GetPhrase());
        }
        
        // Предполагается, что эти действия выполняются передачей с помощью жеста

        /*
            Тест 6: Проверка действия ProcessFrame для добавления буквы
        */
        [Fact]
        public void Writer_Test6()
        {
            var writer = new GestureWriter();
            writer.ProcessFrame("А", "записать");
            Assert.Equal("А", writer.GetPhrase());
        }
        
        /*
            Тест 7: Проверка действия ProcessFrame для добавления пробела
        */
        [Fact]
        public void Writer_Test7()
        {
            var writer = new GestureWriter();
            writer.ProcessFrame("А", "пробел");
            Assert.Equal(" ", writer.GetPhrase());
        }
        
        /*
            Тест 8: Проверка действия ProcessFrame для удаления буквы
        */
        [Fact]
        public void Writer_Test8()
        {
            var writer = new GestureWriter();
            writer.Write("А");
            writer.ProcessFrame("Б", "стереть");
            Assert.Equal("", writer.GetPhrase());
        }
        
        /*
            Тест 9: Проверка игнорирования пустого жеста (не должен записываться)
        */
        [Fact]
        public void Writer_Test9()
        {
            var writer = new GestureWriter();
            writer.ProcessFrame("no_event", "записать");
            Assert.Equal("", writer.GetPhrase());
        }
        
        /*
            Тест 10: Проверка попытки стереть что-то из пустой строки. 
            После попытки, пробуем добавить букву в строку.
        */
        [Fact]
        public void Writer_Test10()
        {
            var writer = new GestureWriter();
            writer.Erase(); 
            Assert.Equal("", writer.GetPhrase());
            
            writer.Write("А");
            Assert.Equal("А", writer.GetPhrase());
        }
        
        /*
            Тест 11: Проверка попытки применить неизвестное действие.
        */
        [Fact]
        public void Writer_Test11()
        {
            var writer = new GestureWriter();
            writer.Write("А");
    
            writer.ProcessFrame("Б", "умножить");
            Assert.Equal("А", writer.GetPhrase()); 
        }
        
        /*
            Тест 12: Проверка совместной работы, когда действие передаются кнопкой или рукой. 
        */
        [Fact]
        public void Writer_Test12()
        {
            var writer = new GestureWriter();
    
            writer.Write("А");
            writer.ProcessFrame("Б", "записать");
            writer.Write("В");
            writer.ProcessFrame("", "пробел");
            writer.ProcessFrame("Г", "записать");
    
            Assert.Equal("АБВ Г", writer.GetPhrase());
        }
    }
}
