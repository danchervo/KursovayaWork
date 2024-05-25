using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Итоговая_Курсовая_Работа
{
    public partial class Form1 : Form
    {
        private List<Doctor> doctors;

        public Form1()
        {
            InitializeComponent();
            doctors = new List<Doctor>();
        }
        public class Doctor
        {
            // Ініціали лікаря
            public string Initials { get; set; }
            // Спеціалізація лікаря
            public string Specialization { get; set; }
            // Номер кабінету, в якому приймає лікар
            public string NumberOfCabinet { get; set; }
            // День прийому лікаря
            public string Day { get; set; }
            // Час прийому лікаря
            public string Time { get; set; }

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void addToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи всі текстові поля заповнені
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                // Якщо одне або більше полів порожні, показуємо повідомлення користувачу
                MessageBox.Show("Заполните все поля!");
                return;
            }
            // Створюємо об'єкт Doctor з даних, введених користувачем
            Doctor doctor = new Doctor
            {
                Initials = textBox1.Text,
                Specialization = textBox2.Text,
                NumberOfCabinet = textBox3.Text,
                Day = textBox4.Text,
                Time = textBox5.Text
            };
            // Перевіряємо, чи існує вже запис з такими ж даними в DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == doctor.Initials &&
                    row.Cells[1].Value != null && row.Cells[1].Value.ToString() == doctor.Specialization &&
                    row.Cells[2].Value != null && row.Cells[2].Value.ToString() == doctor.NumberOfCabinet &&
                    row.Cells[3].Value != null && row.Cells[3].Value.ToString() == doctor.Day &&
                    row.Cells[4].Value != null && row.Cells[4].Value.ToString() == doctor.Time)
                {
                    // Якщо такі дані вже існують, виводимо повідомлення про помилку і виходимо з методу
                    MessageBox.Show("Данные врача уже существуют в таблице.");
                    return;
                }
            }
            // Додаємо нового лікаря до списку лікарів
            doctors.Add(doctor);
            // Додаємо дані нового лікаря до DataGridView
            AddDoctorToDataGridView(doctor);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи вибрано хоча б один рядок у DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Отримуємо індекс першого вибраного рядка
                int index = dataGridView1.SelectedRows[0].Index;
                // Видаляємо рядок з DataGridView за отриманим індексом
                dataGridView1.Rows.RemoveAt(index);
                // Видаляємо лікаря з списку doctors за тим же індексом
                doctors.RemoveAt(index);

            }
            else
            {
                // Якщо жоден рядок не вибрано, показуємо повідомлення користувачу
                MessageBox.Show("Выберите строку для удаления!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи вибрано хоча б один рядок у DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Отримуємо індекс першого вибраного рядка
                int index = dataGridView1.SelectedRows[0].Index;
                // Отримуємо об'єкт Doctor з списку doctors за отриманим індексом
                Doctor doctor = doctors[index];
                // Оновлюємо властивості об'єкта Doctor з даних, введених користувачем
                doctor.Initials = textBox1.Text;
                doctor.Specialization = textBox2.Text;
                doctor.NumberOfCabinet = textBox3.Text;
                doctor.Day = textBox4.Text;
                doctor.Time = textBox5.Text;
                // Оновлюємо дані в DataGridView для вибраного рядка
                UpdateDoctorInDataGridView(index, doctor);
            }
            else
            {
                // Якщо жоден рядок не вибрано, показуємо повідомлення користувачу
                MessageBox.Show("Выберите строку для редактирования!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи є хоча б один запис у списку doctors
            if (doctors.Count > 0)
            {
                // Сортуємо список doctors за ініціалами лікарів у алфавітному порядку
                doctors.Sort((x, y) => string.Compare(x.Initials, y.Initials, StringComparison.Ordinal));
                // Оновлюємо дані у DataGridView після сортування
                RefreshDataGridView();
            }
            else
            {
                // Якщо список doctors порожній, показуємо повідомлення користувачу
                MessageBox.Show("Нет данных для сортировки.");
            }
        }

        private void saveXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Створюємо новий DataSet
            try
            {
                DataSet ds = new DataSet();
                // Створюємо нову DataTable з назвою "Employee"
                DataTable dt = new DataTable { TableName = "Employee" };
                // Додаємо колонки до DataTable
                dt.Columns.Add("Initials");
                dt.Columns.Add("Specialization");
                dt.Columns.Add("NumberOfCabinet");
                dt.Columns.Add("Day");
                dt.Columns.Add("Time");
                // Додаємо DataTable до DataSet
                ds.Tables.Add(dt);
                // Додаємо дані про кожного лікаря до DataTable
                foreach (Doctor doctor in doctors)
                {
                    // Створюємо новий рядок DataRow
                    DataRow row = dt.NewRow();
                    // Заповнюємо рядок даними з об'єкта Doctor
                    row["Initials"] = doctor.Initials;
                    row["Specialization"] = doctor.Specialization;
                    row["NumberOfCabinet"] = doctor.NumberOfCabinet;
                    row["Day"] = doctor.Day;
                    row["Time"] = doctor.Time;
                    // Додаємо заповнений рядок до DataTable
                    dt.Rows.Add(row);
                }
                // Записуємо DataSet у XML файл за вказаним шляхом
                ds.WriteXml(@"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml");
                // Показуємо повідомлення про успішне збереження
                MessageBox.Show("XML файл успешно сохранен.");
            }
            catch
            {
                // Показуємо повідомлення про помилку у випадку виключення
                MessageBox.Show("Невозможно созранить хмл файл!");
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            // Очищаємо всі рядки у DataGridView
            doctors.Clear();
            // Очищаємо список лікарів
        }

        private void deleteTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи є хоча б один запис у списку doctors
            if (doctors.Count > 0)
            {
                // Очищаємо всі рядки у DataGridView
                dataGridView1.Rows.Clear();
                // Очищаємо список лікарів
                doctors.Clear();
                // Шлях до XML-файлу
                string filePath = @"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml";
                // Перевіряємо, чи існує XML-файл за вказаним шляхом
                if (File.Exists(filePath))
                {
                    // Видаляємо XML-файл
                    File.Delete(filePath);
                    // Показуємо повідомлення про успішне видалення даних із таблиці та XML-файлу
                    MessageBox.Show("Данные удалены из таблицы и XML-файл удален.");
                }
                else
                {
                    // Якщо XML-файл не знайдено, показуємо повідомлення про те, що таблиця очищена, але XML-файл не знайдено
                    MessageBox.Show("Таблица очищена, но XML файл не найден.");
                }
            }
            else
            {
                // Якщо список doctors порожній, показуємо повідомлення про те, що таблиця вже порожня
                MessageBox.Show("Таблица уже пустая!");
            }
        }

        private void dowloadXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Очищаємо всі рядки у DataGridView
            dataGridView1.Rows.Clear();

            // Очищаємо список лікарів
            doctors.Clear();

            // Шлях до XML-файлу
            string filePath = @"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml";

            // Перевіряємо, чи існує XML-файл за вказаним шляхом
            if (File.Exists(filePath))
            {
                // Створюємо новий DataSet
                DataSet ds = new DataSet();

                // Зчитуємо дані з XML-файлу у DataSet
                ds.ReadXml(filePath);

                // Перевіряємо, чи є дані у DataSet
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Отримуємо першу таблицю з DataSet
                    DataTable dt = ds.Tables[0];

                    // Надаємо таблиці ім'я "Employee"
                    dt.TableName = "Employee";

                    // Додаємо дані про кожного лікаря з таблиці у список лікарів та у DataGridView
                    foreach (DataRow item in dt.Rows)
                    {
                        Doctor doctor = new Doctor
                        {
                            Initials = item["Initials"].ToString(),
                            Specialization = item["Specialization"].ToString(),
                            NumberOfCabinet = item["NumberOfCabinet"].ToString(),
                            Day = item["Day"].ToString(),
                            Time = item["Time"].ToString()
                        };

                        doctors.Add(doctor);
                        AddDoctorToDataGridView(doctor);
                    }
                }
                else
                {
                    // Показуємо повідомлення, якщо файл XML порожній або містить некоректні дані
                    MessageBox.Show("Файл XML порожній або містить некоректні дані.");
                }
            }
            else
            {
                // Показуємо повідомлення, якщо файл XML не знайдено
                MessageBox.Show("XML файл не знайдено.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Отримуємо спеціалізацію лікаря з текстового поля та видаляємо зайві пробіли
            string specialization = textBox6.Text.Trim();

            // Фільтруємо лікарів за спеціалізацією
            var filteredDoctors = doctors.Where(d => d.Specialization == specialization).ToList();

            // Очищаємо всі рядки у DataGridView
            dataGridView1.Rows.Clear();

            // Додаємо до DataGridView відфільтрованих лікарів
            foreach (var doctor in filteredDoctors)
            {
                AddDoctorToDataGridView(doctor);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Передбачається, що дні тижня знаходяться у четвертому стовпці (індекс 3)
            int columnIndex = 3;

            // Перевіряємо, чи є дані в DataGridView та чи існує стовпець з вказаним індексом
            if (dataGridView1.Rows.Count > 0 && dataGridView1.Columns.Count > columnIndex)
            {
                // Проходимо через кожну комірку у вказаному стовпці та явно перетворюємо її значення на рядок
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[columnIndex].Value = row.Cells[columnIndex].Value?.ToString(); // Додаємо перевірку на null, щоб уникнути винятків
                }

                // Тепер можна сортувати стовпець з днями тижня
                dataGridView1.Sort(dataGridView1.Columns[columnIndex], ListSortDirection.Ascending);
            }
            else
            {
                // Обробка ситуації, коли в DataGridView немає даних або вказаний стовпець не існує
                MessageBox.Show("Немає даних для сортування або вказаний стовпець не існує.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Отримуємо ініціали лікаря з текстового поля та видаляємо зайві пробіли
            string initials = textBox7.Text.Trim();

            // Фільтруємо лікарів за ініціалами
            var filteredDoctors = doctors.Where(d => d.Initials == initials).ToList();

            // Очищаємо всі рядки у DataGridView
            dataGridView1.Rows.Clear();

            // Додаємо до DataGridView відфільтрованих лікарів
            foreach (var doctor in filteredDoctors)
            {
                AddDoctorToDataGridView(doctor);
            }
        }

        private void sortToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Перевіряємо, чи є хоча б один лікар у списку doctors
            if (doctors.Count > 0)
            {
                // Викликаємо алгоритм швидкого сортування для списку лікарів
                QuickSort(doctors, 0, doctors.Count - 1);

                // Оновлюємо дані у DataGridView після сортування
                RefreshDataGridView();
            }
            else
            {
                // Якщо список doctors порожній, виводимо повідомлення про це
                MessageBox.Show("Немає даних для сортування.");
            }
        }

        private void QuickSort(List<Doctor> list, int low, int high)
        {
            // Перевіряємо, чи існує більше одного елемента у списку
            if (low < high)
            {
                // Знаходимо індекс опорного елемента після розділення списку
                int pivotIndex = Partition(list, low, high);

                // Рекурсивно сортуємо ліву частину списку
                QuickSort(list, low, pivotIndex - 1);

                // Рекурсивно сортуємо праву частину списку
                QuickSort(list, pivotIndex + 1, high);
            }
        }

        private int Partition(List<Doctor> list, int low, int high)
        {
            // Визначаємо опорний елемент як останній елемент у списку
            string pivot = list[high].Initials;

            // Ініціалізуємо індекс для майбутнього опорного елемента
            int i = low - 1;

            // Проходимо через усі елементи в межах визначеного діапазону
            for (int j = low; j < high; j++)
            {
                // Порівнюємо ініціали поточного елемента з опорним
                if (string.Compare(list[j].Initials, pivot, StringComparison.Ordinal) < 0)
                {
                    // Якщо ініціали поточного елемента менше опорного,
                    // збільшуємо індекс та міняємо місцями елементи
                    i++;
                    Swap(list, i, j);
                }
            }

            // Міняємо місцями опорний елемент та елемент, розташований після всіх менших за нього
            Swap(list, i + 1, high);

            // Повертаємо індекс нового розташування опорного елемента
            return i + 1;
        }

        private void Swap(List<Doctor> list, int a, int b)
        {
            // Зберігаємо значення елементу з індексом 'a' у тимчасовій змінній
            var temp = list[a];

            // Переміщаємо значення елементу з індексом 'b' на місце елементу з індексом 'a'
            list[a] = list[b];

            // Переміщаємо значення з тимчасової змінної (значення елементу з індексом 'a') на місце елементу з індексом 'b'
            list[b] = temp;
        }

        private void AddDoctorToDataGridView(Doctor doctor)
        {
            // Додаємо новий рядок у DataGridView1 та заповнюємо його даними про лікаря
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = doctor.Initials;
            dataGridView1.Rows[n].Cells[1].Value = doctor.Specialization;
            dataGridView1.Rows[n].Cells[2].Value = doctor.NumberOfCabinet;
            dataGridView1.Rows[n].Cells[3].Value = doctor.Day;
            dataGridView1.Rows[n].Cells[4].Value = doctor.Time;

            // Додаємо новий рядок у DataGridView2 та заповнюємо його даними про лікаря
            int c = dataGridView2.Rows.Add();
            dataGridView2.Rows[n].Cells[0].Value = doctor.Initials;
            dataGridView2.Rows[n].Cells[1].Value = doctor.Specialization;
            dataGridView2.Rows[n].Cells[2].Value = doctor.NumberOfCabinet;
            dataGridView2.Rows[n].Cells[3].Value = doctor.Day;
            dataGridView2.Rows[n].Cells[4].Value = doctor.Time;
        }

        private void UpdateDoctorInDataGridView(int index, Doctor doctor)
        {
            // Оновлюємо дані про лікаря у вказаному рядку DataGridView1
            dataGridView1.Rows[index].Cells[0].Value = doctor.Initials;
            dataGridView1.Rows[index].Cells[1].Value = doctor.Specialization;
            dataGridView1.Rows[index].Cells[2].Value = doctor.NumberOfCabinet;
            dataGridView1.Rows[index].Cells[3].Value = doctor.Day;
            dataGridView1.Rows[index].Cells[4].Value = doctor.Time;
        }

        private void RefreshDataGridView()
        {
            // Очищаємо всі рядки у DataGridView1
            dataGridView1.Rows.Clear();

            // Додаємо усіх лікарів зі списку doctors до DataGridView1
            foreach (var doctor in doctors)
            {
                AddDoctorToDataGridView(doctor);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            // Отримуємо значення спеціалізації з текстового поля та видаляємо зайві пробіли
            string spc = textBox6.Text.Trim();

            // Створюємо список для зберігання знайдених лікарів
            List<string[]> filteredDoctors = new List<string[]>();

            // Проходимо через кожний рядок у dataGridView1
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Отримуємо значення спеціалізації з другої комірки поточного рядка
                string rowSpecialization = row.Cells[1].Value?.ToString();

                // Перевіряємо, чи спеціалізація в поточному рядку дорівнює значенню з textBox6
                if (rowSpecialization == spc)
                {
                    // Якщо так, додаємо рядок до списку знайдених лікарів
                    filteredDoctors.Add(new string[] {
                row.Cells[0].Value.ToString(),
                row.Cells[1].Value.ToString(),
                row.Cells[2].Value.ToString(),
                row.Cells[3].Value.ToString(),
                row.Cells[4].Value.ToString()
            });
                }
            }

            // Очищаємо dataGridView1 перед заповненням новими значеннями
            dataGridView1.Rows.Clear();

            // Заповнюємо dataGridView1 значеннями зі списку знайдених лікарів
            foreach (var doctor in filteredDoctors)
            {
                dataGridView1.Rows.Add(doctor);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void downloadXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Очищаємо всі рядки у DataGridView та список лікарів
            dataGridView1.Rows.Clear();
            doctors.Clear();

            // Перевіряємо, чи існує файл XML за вказаним шляхом
            if (File.Exists(@"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml"))
            {
                // Створюємо новий DataSet для зберігання даних з файлу XML
                DataSet ds = new DataSet();
                // Зчитуємо дані з XML файлу у DataSet
                ds.ReadXml(@"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml");

                // Перевіряємо, чи містить DataSet хоча б одну таблицю та чи вона має хоча б один рядок
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Отримуємо таблицю з DataSet
                    DataTable dt = ds.Tables[0];
                    // Встановлюємо ім'я таблиці як "Employee"
                    dt.TableName = "Employee";

                    // Проходимо через кожний рядок таблиці
                    foreach (DataRow item in dt.Rows)
                    {
                        // Створюємо новий об'єкт лікаря з даних рядка
                        Doctor doctor = new Doctor
                        {
                            Initials = item["Initials"].ToString(),
                            Specialization = item["Specialization"].ToString(),
                            NumberOfCabinet = item["NumberOfCabinet"].ToString(),
                            Day = item["Day"].ToString(),
                            Time = item["Time"].ToString()
                        };

                        // Додаємо лікаря до списку лікарів
                        doctors.Add(doctor);
                        // Додаємо лікаря до DataGridView
                        AddDoctorToDataGridView(doctor);
                    }
                }
                else
                {
                    // Виводимо повідомлення про те, що файл XML порожній або містить некоректні дані
                    MessageBox.Show("Файл XML пустий або містить некоректні дані.");
                }
            }
            else
            {
                // Виводимо повідомлення про те, що файл XML не знайдено
                MessageBox.Show("Файл XML не знайдено.");
            }
        }

        private void saveXMLToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Створюємо новий DataSet для зберігання даних
                DataSet ds = new DataSet();
                // Створюємо нову таблицю з ім'ям "Employee"
                DataTable dt = new DataTable { TableName = "Employee" };
                // Додаємо стовпці до таблиці
                dt.Columns.Add("Initials");
                dt.Columns.Add("Specialization");
                dt.Columns.Add("NumberOfCabinet");
                dt.Columns.Add("Day");
                dt.Columns.Add("Time");
                // Додаємо таблицю до DataSet
                ds.Tables.Add(dt);

                // Проходимо через кожного лікаря у списку doctors
                foreach (Doctor doctor in doctors)
                {
                    // Створюємо новий рядок у таблиці та заповнюємо його даними про лікаря
                    DataRow row = dt.NewRow();
                    row["Initials"] = doctor.Initials;
                    row["Specialization"] = doctor.Specialization;
                    row["NumberOfCabinet"] = doctor.NumberOfCabinet;
                    row["Day"] = doctor.Day;
                    row["Time"] = doctor.Time;
                    // Додаємо рядок до таблиці
                    dt.Rows.Add(row);
                }

                // Записуємо дані з DataSet у XML файл
                ds.WriteXml(@"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml");
                MessageBox.Show("XML файл успішно збережено.");
            }
            catch
            {
                // Виводимо повідомлення про неможливість збереження XML файлу у випадку виникнення помилки
                MessageBox.Show("Неможливо зберегти XML файл!");
            }
        }

        private void downloadXMLToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Очищаємо всі рядки у DataGridView2 та список лікарів
            dataGridView2.Rows.Clear();
            doctors.Clear();

            // Перевіряємо, чи існує файл XML за вказаним шляхом
            if (File.Exists(@"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml"))
            {
                // Створюємо новий DataSet для зберігання даних з файлу XML
                DataSet ds = new DataSet();
                // Зчитуємо дані з XML файлу у DataSet
                ds.ReadXml(@"C:\C#\KursovayaRabotaUMSF\XMLFile1.xml");

                // Перевіряємо, чи містить DataSet хоча б одну таблицю та чи вона має хоча б один рядок
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Отримуємо таблицю з DataSet
                    DataTable dt = ds.Tables[0];
                    // Встановлюємо ім'я таблиці як "Employee"
                    dt.TableName = "Employee";

                    // Проходимо через кожний рядок таблиці
                    foreach (DataRow item in dt.Rows)
                    {
                        // Створюємо новий об'єкт лікаря з даних рядка
                        Doctor doctor = new Doctor
                        {
                            Initials = item["Initials"].ToString(),
                            Specialization = item["Specialization"].ToString(),
                            NumberOfCabinet = item["NumberOfCabinet"].ToString(),
                            Day = item["Day"].ToString(),
                            Time = item["Time"].ToString()
                        };

                        // Додаємо лікаря до списку лікарів
                        doctors.Add(doctor);
                        // Додаємо лікаря до DataGridView2
                        AddDoctorToDataGridView(doctor);
                    }
                }
                else
                {
                    // Виводимо повідомлення про те, що файл XML порожній або містить некоректні дані
                    MessageBox.Show("Файл XML порожній або містить некоректні дані.");
                }
            }
            else
            {
                // Виводимо повідомлення про те, що файл XML не знайдено
                MessageBox.Show("Файл XML не знайдено.");
            }
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Очищаємо всі рядки у DataGridView2 та список лікарів
            dataGridView2.Rows.Clear();
            doctors.Clear();
        }

        private void binarySearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Отримуємо значення ініціалів для пошуку з текстового поля та видаляємо зайві пробіли
            string searchInitials = textBox8.Text.Trim();

            // Перевіряємо, чи введено значення для пошуку
            if (!string.IsNullOrEmpty(searchInitials))
            {
                // Виконуємо бінарний пошук за ініціалами у списку лікарів
                int index = BinarySearch(doctors, searchInitials);

                // Перевіряємо, чи знайдено лікаря з такими ініціалами
                if (index != -1)
                {
                    // Скидаємо виділення у DataGridView2 та виділяємо рядок з знайденим лікарем
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows[index].Selected = true;
                    // Прокручуємо до першого видимого рядка, щоб показати знайдений лікар
                    dataGridView2.FirstDisplayedScrollingRowIndex = index;
                }
                else
                {
                    // Виводимо повідомлення, що лікар з такими ініціалами не знайдено
                    MessageBox.Show("Лікар з такими ініціалами не знайдено.");
                }
            }
            else
            {
                // Виводимо повідомлення про необхідність введення ініціалів для пошуку
                MessageBox.Show("Введіть ініціали для пошуку.");
            }
        }
        private int BinarySearch(List<Doctor> list, string initials)
        {
            // Ініціалізуємо змінні для нижньої та верхньої меж пошуку
            int low = 0, high = list.Count - 1;

            // Проводимо пошук методом бінарного пошуку
            while (low <= high)
            {
                // Знаходимо середину діапазону
                int mid = (low + high) / 2;

                // Порівнюємо ініціали лікаря у середині діапазону з введеними ініціалами
                int comparison = string.Compare(list[mid].Initials, initials, StringComparison.Ordinal);

                // Якщо знайдено відповідність, повертаємо індекс лікаря
                if (comparison == 0)
                {
                    return mid;
                }

                // Якщо введені ініціали більше, ніж ініціали лікаря у середині діапазону, змінюємо нижню межу
                if (comparison < 0)
                {
                    low = mid + 1;
                }
                // Якщо введені ініціали менше, ніж ініціали лікаря у середині діапазону, змінюємо верхню межу
                else
                {
                    high = mid - 1;
                }
            }

            // Повертаємо -1, якщо лікар з введеними ініціалами не знайдено
            return -1;
        }

        private void quickSortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи є дані для сортування
            if (doctors.Count > 0)
            {
                // Викликаємо алгоритм швидкого сортування для списку лікарів
                QuickSort(doctors, 0, doctors.Count - 1);
                // Оновлюємо DataGridView після сортування
                RefreshDataGridView();
            }
            else
            {
                // Виводимо повідомлення, що немає даних для сортування
                MessageBox.Show("Немає даних для сортування.");
            }
        }
    }
}
