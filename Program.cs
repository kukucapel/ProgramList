using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace _1_0
{
    class Interface
    {
        public void HelloInterface()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("=======Product DataBase======");
            Console.WriteLine("=============================");
            Console.WriteLine("========Input any key========");
            Console.ReadKey();
            Console.Clear();
        }
        public void ConnectTrueDbInterface()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("======Connect to DataBase====");
            Console.WriteLine("==========Successful=========");
            Console.WriteLine("=============================");
            Console.WriteLine("========Input any key========");
            Console.ReadKey();
            Console.Clear();
        }
        public void ConnectFalseInterface()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("======Connect to DataBase====");
            Console.WriteLine("============Failed===========");
            Console.WriteLine("=============================");
            Console.WriteLine("Enter another database server:"); 
        }
        public void MainMenu()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("===1) Order menu=============");
            Console.WriteLine("===2) DataBase menu==========");
            Console.WriteLine("=============================");
            Console.WriteLine("===e) exit===================");
            Console.WriteLine("=============================");
        }
        public void OrderMenu()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("===1) Show orders============");
            Console.WriteLine("===2) Add order==============");
            Console.WriteLine("===3) Edit order=============");
            Console.WriteLine("===4) Check time=============");
            Console.WriteLine("===5) Cancel order===========");
            Console.WriteLine("=============================");
            Console.WriteLine("===b) back===================");
            Console.WriteLine("=============================");
        }
        public void ShowOrderMenu()
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("=========1) Input number order========================");
            Console.WriteLine("============to show order list========================");
            Console.WriteLine("=========0) back======================================");
            Console.WriteLine("======================================================");
        }
        public void AddListOrderMenu()
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("=========1) Input number order========================");
            Console.WriteLine("============to add list to order======================");
            Console.WriteLine("=========0) back======================================");
            Console.WriteLine("======================================================");
        }
        public void DataBaseMenu()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("===1) Show area==============");
            Console.WriteLine("===2) Show list==============");
            Console.WriteLine("===3) Show free==============");
            Console.WriteLine("===4) Edit status============");
            Console.WriteLine("=============================");
            Console.WriteLine("===b) back===================");
            Console.WriteLine("=============================");
        }
        public void CheckTime()
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("=========1) Input number order========================");
            Console.WriteLine("============to check time for production==============");
            Console.WriteLine("=========0) back======================================");
            Console.WriteLine("======================================================");
        }
        public void CancelOrder()
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("=========1) Input number order========================");
            Console.WriteLine("============to cancel order===========================");
            Console.WriteLine("=========0) back======================================");
            Console.WriteLine("======================================================");
        }
        public void EditStatus()
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("=========1) Input number area=========================");
            Console.WriteLine("============to complate the current task==============");
            Console.WriteLine("=========0) back======================================");
            Console.WriteLine("======================================================");
        }
    }
    class Program
    {
        static void Ready(SqlConnection connection, int id)
        {
            int orderId = -1;
            List<int> idListOrder = new List<int>();
            List<int> statusList = new List<int>();
            List<int> idList = new List<int>();
            SqlCommand command = new SqlCommand($"EXEC ReadyListSteep1 {id};", connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        orderId = reader.GetInt32(0);
                    }
                }
            }
            catch
            {
                reader.Close();
            }
            reader.Close();
            command.CommandText = $"EXEC ReadyListSteep2 {orderId};";
            reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idListOrder.Add(reader.GetInt32(0));
                        statusList.Add(reader.GetInt32(1));
                    }
                }
            }
            catch
            {
                reader.Close();
            }
            reader.Close();
            command.CommandText = $"EXEC ReadyListSteep3 {id};";
            reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idList.Add(reader.GetInt32(0));
                    }
                }
            }
            catch
            {
                reader.Close();
            }
            reader.Close();
            for (int i = 0; i < idListOrder.Count; i++)
            {
                if(statusList[i] == 1 && idList.Contains(idListOrder[i]))
                {
                    command.CommandText = $"EXEC ReadyListSteep4 {orderId}, {idListOrder[i]}, {id};";
                    command.ExecuteNonQuery();
                    command.CommandText = $"EXEC ReadyListSteep5 {idListOrder[i]}, {orderId}, {id}";
                    command.ExecuteNonQuery();
                }
            }
            //Console.WriteLine(orderId);
            //Console.WriteLine(string.Join(", ", idListOrder));
            //Console.WriteLine(string.Join(", ", statusList));
            //Console.WriteLine(string.Join(", ", idList));
            //Console.ReadKey();
            Console.Clear();
        }
        static void CheckTime(SqlConnection connection, int id)
        {
            int sumTime = 0;
            int timeOrder = 0;
            SqlCommand command = new SqlCommand($"EXEC ReturnTimeOrderListProduction {id};", connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sumTime += reader.GetInt32(0);
                    }
                }
            }
            catch
            {
                reader.Close();
            }
            reader.Close();
            timeOrder = ReturnTimeOrder(connection, id);
            if(sumTime > timeOrder)
            {
                Console.WriteLine("The order will be made on time");
            }
            else
            {
                Console.WriteLine("The order will not be made on time");
            }
            Console.WriteLine($"Order time (hours): {timeOrder}");
            Console.WriteLine($"Order completion time of list (hour): {sumTime}");
            Console.ReadKey();
            Console.Clear();

        }
        static void CheckReady(SqlConnection connection)
        {
            List<int> status = new List<int>();
            List<int> idOrders = new List<int>();
            List<int> statusOrder = new List<int>();
            SqlCommand command = new SqlCommand("EXEC CheckSteep1", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    idOrders.Add(reader.GetInt32(0));
                    try
                    {
                        statusOrder.Add(Convert.ToInt32(reader.GetValue(1)));
                    }
                    catch
                    {
                        statusOrder.Add(-1);
                    }
                }
            }
            reader.Close();

            for (int i = 0; i < idOrders.Count; i++)
            {
                if (statusOrder[i] == 0)
                {
                    command.CommandText = $"EXEC CheckSteep2 {idOrders[i]};";
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            status.Add(reader.GetInt32(0));
                        }
                    }
                    reader.Close();
                    if(status.Count != 0)
                    {
                        //Console.WriteLine(string.Join(", ", status));
                        //Console.WriteLine(status.Contains(0));
                        //Console.WriteLine(status.Contains(1));
                        if (!(status.Contains(0) || status.Contains(1)))
                        {
                            command.CommandText = $"UPDATE Orders SET status_order = 1 WHERE id = {idOrders[i]};";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText = $"UPDATE Orders SET status_order = 0 WHERE id = {idOrders[i]};";
                            command.ExecuteNonQuery();
                        }
                        status.Clear();
                    }
                    
                }
                reader.Close();

            }
            reader.Close();
            command.CommandText = "EXEC CheckCount;";
            command.ExecuteNonQuery();
            //Console.ReadKey();
        }
        static void CancelOrder(SqlConnection connection, int id_order)
        {
            SqlCommand command = new SqlCommand($"Exec CancelOrder {id_order};", connection);
            command.ExecuteNonQuery();
            command.CommandText = $"DELETE Complate WHERE id_order = {id_order};";
            command.ExecuteNonQuery();
            Console.Clear();
        }
        static void WorkBd(SqlConnection connection)
        {
            List<int> idOrders = new List<int>();
            List<int> idListByIdOrder = new List<int>();
            List<int> free = new List<int>();
            List<int> statusOrderList = new List<int>();
            int idArea = -1;
            int statusArea = 0;
            
            SqlCommand command = new SqlCommand("EXEC WorkSteep1", connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idOrders.Add(reader.GetInt32(0));
                    }
                }
                reader.Close();
            }
            catch
            {
                reader.Close();
            }
            command.CommandText = "EXEC WorkSteep3";
            reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        free.Add(reader.GetInt32(0));
                    }
                }
                reader.Close();
            }
            catch
            {
                reader.Close();
            }
            for (int i = 0; i < idOrders.Count; i++)
            {

                command.CommandText = $"EXEC WorkSteep2 {idOrders[i]}";
                SqlDataReader reader1 = command.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        idListByIdOrder.Add(reader1.GetInt32(0));
                    }
                }
                reader1.Close();
                command.CommandText = $"SELECT status_list FROM OrderList WHERE id_order = {idOrders[i]};";
                reader1 = command.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        statusOrderList.Add(reader1.GetInt32(0));
                    }
                }
                reader1.Close();
                
                //Console.WriteLine(string.Join(", ", idListByIdOrder));
                //Console.WriteLine(string.Join(", ", statusOrderList));
                for (int j = 0; j < idListByIdOrder.Count; j++)
                {
                    command.CommandText = $"EXEC WorkSteep4 {idListByIdOrder[j]};";
                    reader1 = command.ExecuteReader();
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            idArea = reader1.GetInt32(0);
                        }
                    }
                    reader1.Close();
                    for (int k = 0; k < free.Count; k++)
                    {

                        if (idListByIdOrder[j] == free[k] && statusOrderList[j] != 2)
                        {
                            command.CommandText = $"EXEC ReadyListSteep4 {idOrders[i]}, {idListByIdOrder[j]}, {idArea};";
                            command.ExecuteNonQuery();
                            command.CommandText = $"DELETE TOP(1) FROM Free WHERE id_list = {free[k]};";
                            command.ExecuteNonQuery();
                            command.CommandText = $"UPDATE TOP(1) OrderList SET  status_list = 2 WHERE id_list = {free[k]} AND id_order = {idOrders[i]} AND status_list != 2";
                            command.ExecuteNonQuery();
                            free.RemoveAt(k);
                            idListByIdOrder[j] = -1;
                            break;
                        }
                    }
                }
                for (int j = 0; j < idListByIdOrder.Count; j++)
                {
                    if(idListByIdOrder[j] != -1)
                    {
                        command.CommandText = $"EXEC WorkSteep4 {idListByIdOrder[j]};";
                        reader1 = command.ExecuteReader();
                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                idArea = reader1.GetInt32(0);
                            }
                        }
                        reader1.Close();
                        command.CommandText = $"EXEC WorkSteep5 {idArea};";
                        reader1 = command.ExecuteReader();
                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                try
                                {
                                    statusArea = reader1.GetInt32(0);
                                }
                                catch
                                {
                                    statusArea = 0;
                                }
                            }
                        }
                        reader1.Close();
                        if(statusOrderList[j] == 0 && statusArea == 0)
                        {
                            command.CommandText = $"EXEC WorkSteep6 {idOrders[i]}, {idArea}, {idListByIdOrder[j]};";
                            command.ExecuteNonQuery();
                        }
                    }
                    
                }
                statusOrderList.Clear();
                idListByIdOrder.Clear();
            }

            //Console.WriteLine(string.Join(", ", idOrders));
            //Console.WriteLine(string.Join(", ", free));
            //Console.ReadKey();
            Console.Clear();
        }
        static int ReturnTimeOrder(SqlConnection connection, int id)
        {
            int timeOrder = 0;
            SqlCommand command = new SqlCommand($"EXEC ReturnTimeOrder {id};", connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        timeOrder = reader.GetInt32(0);
                    }
                }
            }
            catch
            {
                reader.Close();
            }
            reader.Close();
            return timeOrder;
        }
        static int ShowList(SqlConnection connection)
        {
            string[] names = new string[3];
            object[] values = new object[3];
            int maxId = 0;
            SqlCommand command = new SqlCommand("EXEC ShowLists;", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                for (int i = 0; i < 2; i++)
                {
                    names[i] = reader.GetName(i);

                }
                Console.WriteLine($"{names[0]}\t\t{names[1]}");
                while (reader.Read())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        values[i] = reader.GetValue(i);

                    }
                    maxId = Convert.ToInt32(values[0]);
                    Console.WriteLine($"{values[0]}\t\t{values[1]}");
                }
                reader.Close();
            }
            reader.Close();
            return maxId;
        }
        static int ReturnMaxId(SqlConnection connection, string objectMax)
        {
            int maxId = 0;
            SqlCommand command = new SqlCommand($"EXEC {objectMax};", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    
                    maxId = Convert.ToInt32(reader.GetValue(0));
                    
                }
                //Console.WriteLine($"{maxId}");
                reader.Close();
            }
            reader.Close();
            return maxId;
        }
        static int ShowArea(SqlConnection connection)
        {
            string[] names = new string[5];
            object[] values = new object[5];
            int maxId = 0;
            SqlCommand command = new SqlCommand("EXEC ShowArea;", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                for (int i = 0; i < 5; i++)
                {
                    names[i] = reader.GetName(i);
                    Console.Write($"{names[i],-23}");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        values[i] = reader.GetValue(i);
                        Console.Write($"{values[i],-23}");
                    }
                    maxId = Convert.ToInt32(values[0]);
                    Console.WriteLine();
                }
                reader.Close();
            }
            reader.Close();
            
            return maxId;
        }
        static int ShowOrder(SqlConnection connection)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            int maxId = 0;
            DateTime tempTime = new DateTime();
            SqlCommand command = new SqlCommand("EXEC ShowOrders;", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) 
            {
                for(int i = 0; i < 4; i++)
                {
                    names[i] = reader.GetName(i);
                    Console.Write($"{names[i]}" +  "\t");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    for(int i = 0; i < 4; i++)
                    {
                        var tempObject = reader.GetValue(i);
                        if(tempObject is DateTime) // если дата, нормально выводим в консоль
                        {
                            tempTime = (DateTime)tempObject;
                            values[i] = tempTime.ToShortDateString();
                        }
                        else
                        {
                            values[i] = tempObject.ToString();
                        }
                        Console.Write($"{values[i]}" + "\t");
                    }
                    maxId = Convert.ToInt32(values[0]);
                    Console.WriteLine();
                }
                reader.Close();
            }
            reader.Close();
            return maxId;
        }
        static void ShowFree(SqlConnection connection)
        {
            string[] names = new string[2];
            string[] values = new string[2];
            SqlCommand command = new SqlCommand("EXEC ShowFree;", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                names[0] = reader.GetName(0);
                names[1] = reader.GetName(1);
                Console.WriteLine($"{names[0],-40}\t{names[1]}");
                while (reader.Read())
                {
                    values[0] = reader.GetString(0);
                    values[1] = reader.GetString(1);
                    Console.WriteLine($"{values[0],-40}\t{values[1]}");
                }
            }
            reader.Close();
            Console.ReadKey();
            Console.Clear();
        }
        static void ShowOrderList(SqlConnection connection, int id)
        {
            string name;
            string value;

            SqlCommand command = new SqlCommand($"EXEC ShowOrderListById {id};", connection);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                name = reader.GetName(0);
                Console.WriteLine($"{name, -40}\t{"Status", 15}");
                while (reader.Read())
                {
                    value = reader.GetString(0);
                    Console.Write($"{value, -40}\t");
                    value = Convert.ToString(reader.GetValue(1));
                    Console.WriteLine($"{value, 15}");
                }
                reader.Close();
            }
            else
            {
                Console.WriteLine("No parts or kits on lists");
            }
            reader.Close();
            Console.ReadKey();
            Console.Clear();
        }
        static void AddOrder(SqlConnection connection)
        {
            string dateStart;
            string dateEnd;
            Console.Clear();
            Console.WriteLine("Input date start (year-month-day) (for stop add input 0)");
            dateStart = Console.ReadLine();

            if (dateStart != "0")
            {
                Console.WriteLine("Input date end (year-month-day) (for stop add input 0)");
                dateEnd = Console.ReadLine();
                if (dateEnd != "0")
                {
                    try
                    {
                        string sqlExpression = $"INSERT Orders (date_start, date_end) VALUES ('{dateStart}', '{dateEnd}');";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.ExecuteNonQuery();
                        Console.ReadKey();
                        Console.Clear();
                    }
                    catch
                    {
                        Console.WriteLine("Error, check your data");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            Console.Clear();
        }
        static void AddListByOrderId(SqlConnection connection, int id)
        {
            Console.Clear();
            List<int> values = new List<int>();
            int userInput = -1;
            ShowList(connection);
            Console.WriteLine("Input id of list to add part to order (input 0 to stop)");
            while (userInput != 0)
            {
                userInput = Convert.ToInt32(Console.ReadLine());
                if (userInput <= ReturnMaxId(connection, "ShowLists") && userInput > 0)
                {
                    try
                    {
                        values.Add(userInput);
                    }
                    catch
                    {
                        continue;
                    }
                }   
            }
            for (int i = 0; i < values.Count; i++)
            {
                string sqlExpression = $"INSERT OrderList (id_order, id_list) VALUES ('{id}', '{values[i]}');";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();

            }
            Console.WriteLine(string.Join(", ", values));
            Console.ReadKey();
            Console.Clear();
        }
        static void ConnectBd(string defaultConnectionString)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = defaultConnectionString;
                connection.Open();
                SqlConnection.ClearAllPools();
                connection.Close();
                connection.Dispose();
            }
        }
        static void Main(string[] args)
        {
            
            var defaultConnectionString = "Server=(localdb)\\mssqllocaldb;Database=product;Trusted_Connection=True;";
            char userInput = ' ';
            int userInputId = -1;
            int maxId = 0;
            Interface ProgramInterface = new Interface();           
            ProgramInterface.HelloInterface();
            while (true) // подключение
            {
                try
                {
                    ConnectBd(defaultConnectionString);
                    ProgramInterface.ConnectTrueDbInterface();
                        
                    break;
                }
                catch
                {
                    
                    ProgramInterface.ConnectFalseInterface();
                    defaultConnectionString = Console.ReadLine();
                    Console.Clear();
                }
            }
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = defaultConnectionString;
            connection.Open();
            while (userInput != 'e') //main menu
            {

                CheckReady(connection);
                WorkBd(connection);

                ProgramInterface.MainMenu();
                userInput = Console.ReadKey(true).KeyChar;
                Console.Clear();
                switch (userInput)
                {
                    case '1': //order menu
                        while (userInput != 'b')
                        {
                            try
                            {
                                CheckReady(connection);
                                WorkBd(connection);
                            }
                            catch
                            {

                            }
                            ProgramInterface.OrderMenu();
                            userInput = Console.ReadKey(true).KeyChar;
                            Console.Clear();
                            switch (userInput)
                            {
                                case '1':
                                    userInputId = -1;
                                    while (userInputId != 0)
                                    {
                                        
                                        maxId = ShowOrder(connection);
                                        ProgramInterface.ShowOrderMenu();
                                        try
                                        {
                                            userInputId = Convert.ToInt32(Console.ReadLine());
                                        
                                            if(userInputId > maxId || userInputId <= 0){
                                                Console.Clear();
                                            }
                                            else
                                            {
                                                ShowOrderList(connection, userInputId);
                                            }
                                        }
                                        catch
                                        {
                                            Console.Clear();
                                        }
                                    }
                                    
                                    break;
                                case '2':
                                    AddOrder(connection);
                                    break;
                                case '3':
                                    userInputId = -1;
                                    while (userInputId != 0)
                                    {
                                      
                                        maxId = ShowOrder(connection);
                                        ProgramInterface.ShowOrderMenu();
                                        try
                                        {
                                            userInputId = Convert.ToInt32(Console.ReadLine());
                                            if (userInputId > maxId || userInputId <= 0)
                                            {
                                                Console.Clear();
                                            }
                                            else
                                            {
                                                AddListByOrderId(connection, userInputId);
                                            }
                                        }
                                        catch
                                        {
                                            Console.Clear();
                                        }
                                    }
                                    break;
                                case '4':
                                    userInputId = -1;
                                    while (userInputId != 0)
                                    {
                                        
                                        maxId = ShowOrder(connection);
                                        ProgramInterface.CheckTime();
                                        try
                                        {
                                            userInputId = Convert.ToInt32(Console.ReadLine());
                                            if (userInputId > maxId || userInputId <= 0)
                                            {
                                                Console.Clear();
                                            }
                                            else
                                            {
                                                CheckTime(connection, userInputId);
                                            }
                                        }
                                        catch
                                        {
                                            Console.Clear();
                                        }
                                    }
                                    break;
                                case '5':
                                    userInputId = -1;
                                    while (userInputId != 0)
                                    {
                                        
                                        maxId = ShowOrder(connection);
                                        ProgramInterface.CancelOrder();
                                        try
                                        {
                                            userInputId = Convert.ToInt32(Console.ReadLine());
                                            if (userInputId > maxId || userInputId <= 0)
                                            {
                                                Console.Clear();
                                            }
                                            else
                                            {
                                                CancelOrder(connection, userInputId);
                                            }
                                        }
                                        catch
                                        {
                                            Console.Clear();
                                        }
                                    }
                                    
                                    break;
                            }
                        }
                        break;
                    case '2': //area menu
                        while(userInput != 'b')
                        {
                            try
                            {
                                CheckReady(connection);
                                WorkBd(connection);
                            }
                            catch
                            {

                            }

                            ProgramInterface.DataBaseMenu();
                            userInput = Console.ReadKey(true).KeyChar;
                            Console.Clear();
                            switch (userInput)
                            {
                                case '1':
                                    maxId = ShowArea(connection);
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                case '2':
                                    maxId = ShowList(connection);
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                case '3':
                                    ShowFree(connection);
                                    break;
                                case '4':
                                    userInputId = -1;
                                    while (userInputId != 0)
                                    {
                                       
                                        maxId = ShowArea(connection);
                                        ProgramInterface.EditStatus();
                                        try
                                        {
                                            userInputId = Convert.ToInt32(Console.ReadLine());
                                            if (userInputId > maxId || userInputId <= 0)
                                            {
                                                Console.Clear();
                                            }
                                            else
                                            {
                                                Ready(connection, userInputId);
                                            }
                                        }
                                        catch
                                        {
                                            Console.Clear();
                                        }
                                        try
                                        {
                                            CheckReady(connection);
                                            WorkBd(connection);
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    break;
                                case '5':
                                    CheckReady(connection);
                                    break;
                            }
                        }
                        break;
                    default:
                        continue;
                } 
            }
        }
    }
}
