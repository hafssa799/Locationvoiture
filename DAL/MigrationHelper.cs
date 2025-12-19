using System;
using System.Data.SqlClient;

namespace LocationVoitures.BackOffice.DAL
{
    public static class MigrationHelper
    {
        public static void EnsurePhotoColumnExists()
        {
            try
            {
                var db = new DatabaseHelper();
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    
                    // Check if column exists
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = 'Vehicules' 
                        AND COLUMN_NAME = 'PhotoPath'";

                    using (var cmd = new SqlCommand(checkQuery, conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        
                        if (count == 0)
                        {
                            // Add column
                            string alterQuery = "ALTER TABLE Vehicules ADD PhotoPath NVARCHAR(500) NULL";
                            using (var alterCmd = new SqlCommand(alterQuery, conn))
                            {
                                alterCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently fail or log in a real app. 
                // In dev, we might see it if something major breaks.
                Console.WriteLine("Migration Error: " + ex.Message);
            }
        }
    }
}
