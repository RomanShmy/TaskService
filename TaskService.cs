using System;
using System.Collections.Generic;
using models;
using Npgsql;

namespace Task
{
    public class TaskService
    {
        public string ConnectionString { get; set; }    
    
        public TaskService(string connectionString)    
        {    
            this.ConnectionString = connectionString;    
        }    
    
        private NpgsqlConnection GetConnection()    
        {    
            return new NpgsqlConnection(ConnectionString);    
        } 

        public List<Note> GetAllTasks()
        {
            List<Note> tasks = new List<Note>();
            string query = "select * from fef.tasks";
            using(NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Note()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Done = reader.GetBoolean(2)
                        });
                    }
                }
                
            }

            return tasks;
        }

        public Note SaveTask(Note note)
        {
            string query = "insert into fef.tasks (name, done) values (@name, @done)";
            using(NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("name", note.Name);
                cmd.Parameters.AddWithValue("done", note.Done);
                cmd.ExecuteNonQuery();

            }
            return note;
        }

        public bool UpdateTaskDone(int id, bool done)
        {
            if (IsPresent(id))
            {
                string query = $"update fef.tasks set done={done} where id = {id}";
                using (NpgsqlConnection connection = GetConnection())
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(query, connection);

                    cmd.ExecuteNonQuery();
                }

                return true;
            }

            return false;
        }

        private bool IsPresent(int id)
        {
            Note note = new Note();
            string query = "select * from fef.tasks where id = @id";
            using(NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        note.Id = reader.GetInt32(0);
                        note.Name = reader.GetString(1);
                        note.Done = reader.GetBoolean(2);
                    }
                }
                
            }

            if (note.Name == null)
            {
                return false;
            }

            return true;
        }
    }
}