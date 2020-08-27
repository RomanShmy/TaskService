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

        public Note GetTaskById(int id)
        {
            if(!IsPresent(id))
            {
                return new Note();
            }
            Note task = new Note();
            string query = "select * from fef.tasks where id=@id";
            using(NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        task.Id = reader.GetInt32(0);
                        task.Name = reader.GetString(1);
                        task.Done = reader.GetBoolean(2);
                    }
                }
                
            }

            return task;
        }

        public Note SaveTask(Note note)
        {
            string query = "insert into fef.tasks (name, done) values (@name, @done) returning id";
            using(NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("name", note.Name);
                cmd.Parameters.AddWithValue("done", note.Done);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        note.Id = reader.GetInt32(0);
                    }
                }

            }
            
            return note;
        }

        public bool UpdateTaskDone(int id, Note note)
        {
            if (!IsPresent(id))
            {
                return false;
            }
            string query = "update fef.tasks set done=@done where id=@id";
            using (NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("done", note.Done);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool Delete(int id)
        {
            if (!IsPresent(id))
            {
                return false;
            }

            string query = "delete from fef.tasks where id = @id";
            using(NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        private bool IsPresent(int id)
        {
            bool result = true;
            string query = "select exists(select 1 from fef.tasks where id = @id)";
            using(NpgsqlConnection connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetBoolean(0);
                    }
                }
            }
            return result;
        }
    }
}