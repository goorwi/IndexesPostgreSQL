namespace IndexesPostgreSQL
{
    public class TreeConfiguration
    {
        public TreeConfiguration() { }

        public int BlockWidth { get; set; } = 80; // Пример значения, можно изменить
        public int BlockHeight { get; set; } = 40; // Пример значения, можно изменить
        public int HorizontalSpacing { get; set; } = 20; // Горизонтальное расстояние между блоками
        public int VerticalSpacing { get; set; } = 30; // Вертикальное расстояние между блоками
    }
}
