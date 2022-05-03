using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Supabase;
using Supabase.Realtime;
using Client = Supabase.Client;

namespace TableTest.Model;

public class Database : INotifyPropertyChanged
{
    public Database()
    {
        // В конструкторе создаем массив со студентами,
        // в котором будут храниться все строки из таблицы
        Table = new List<Students>();

        // Подключаемся к базе данных
        string url = "https://tgnneepabdskpnlygcyh.supabase.co";
        string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRnbm5lZXBhYmRza3BubHlnY3loIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTY0NjkyNDg3NCwiZXhwIjoxOTYyNTAwODc0fQ.IsFF4OzwHSBItBObA0jfD1497eI_7UtE3F55WbwY0C8";

        Client.InitializeAsync(url, key, new SupabaseOptions
        {
            AutoConnectRealtime = true,
            ShouldInitializeRealtime = true
        });

        // Получаем экземпляр клиента
        Client = Client.Instance;

        // И подписываемся на события изменения в базе данных
        Client.From<Students>().On(Client.ChannelEventType.All, StudentsChanged);
    }

    // Клиент для обращения к базе данных
    private Client Client { get; }

    // Массив со студентами из базы
    public IEnumerable<Students> Table { get; set; }

    // Событие изменения массива для обновления интерфейса
    public event PropertyChangedEventHandler? PropertyChanged;

    // При изменении данных в талице на сервере просто подгружаем данные из нее
    private void StudentsChanged(object sender, SocketResponseEventArgs a)
    {
        LoadData();
    }

    // А вот так просходит загрузка данных из талицы
    // на сервере Supabase в массив нашей программы
    public async void LoadData()
    {
        // Берем данные из таблицы и помещаем их в массив
        var data = await Client.From<Students>().Get();
        Table = data.Models;

        // Вызов этой функции необходим для автоматического обновления
        // интерфейса программы при изменении данных в массиве со студентами
        OnPropertyChanged(nameof(Table));
    }

    // Реализация интерфейса INotifyPropertyChanged необходима для обновления формы программы
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}