using DataAccess.Interfaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace DataAccess
{
    /// <summary>
    /// A repository for accessing SQLite databases.
    /// </summary>
    public class SQLiteRepository : IDataAccessRepository, IDisposable
    {
        private const string databaseName = "db.sqlite";
        private string connectionPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, SQLiteRepository.databaseName);
        private bool hasBeenDisposed;
        private SQLiteConnection sqliteConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteRepository"/> class.
        /// </summary>
        public SQLiteRepository()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteRepository"/> class.
        /// </summary>
        /// <param name="connectionPath">The connection path.</param>
        public SQLiteRepository(string connectionPath)
        {
            this.connectionPath = connectionPath;
            this.InitializeComponent();
        }

        /// <inheritdoc />
        public void Create<T>() where T : IEntity
        {
            if (this.sqliteConnection.TableMappings.FirstOrDefault(table => table.MappedType.FullName == typeof(T).Name) == null)
            {
                this.sqliteConnection.CreateTable<T>();
            }
        }

        /// <inheritdoc />
        public void Insert<T>(T entity) where T : IEntity
        {
            this.sqliteConnection.Insert(entity);
        }

        /// <inheritdoc />
        public void Update<T>(T entity) where T : IEntity
        {
            if (entity.Id == 0)
            {
                this.sqliteConnection.Insert(entity);
            }
            else
            {
                this.sqliteConnection.Update(entity);
            }
        }

        /// <inheritdoc />
        public void Update<T>(IEnumerable<T> entities) where T : IEntity
        {
            this.sqliteConnection.InsertAll(entities.Where(entity => entity.Id == 0));
            this.sqliteConnection.UpdateAll(entities.Where(entity => entity.Id != 0));
        }

        /// <inheritdoc />
        public void Remove<T>(int id) where T : IEntity
        {
            this.sqliteConnection.Delete<T>(id);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetItems<T>() where T : class, IEntity
        {
            var items = new List<T>();

            if (this.sqliteConnection.TableMappings.FirstOrDefault(table => table.MappedType.FullName == typeof(T).Name) == null)
            {
                items = this.sqliteConnection.Table<T>().ToList();
            }

            return items;
        }

        /// <inheritdoc />
        public void CommitChanges()
        {
            if (this.sqliteConnection != null)
            {
                this.sqliteConnection.Commit();
            }
        }

        private void InitializeComponent()
        {
            this.sqliteConnection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), this.connectionPath);
        }

        #region IDisposable
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SQLiteRepository()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.hasBeenDisposed)
                return;

            if (disposing)
            {
                try
                {
                    this.sqliteConnection.Close();
                    this.sqliteConnection.Dispose();
                }
                catch (Exception ex)
                {
                    this.sqliteConnection = null;
                    System.Diagnostics.Debug.Write("An exception ocurred when disposing the sqlite connection: " + ex.Message);
                }
            }

            this.hasBeenDisposed = true;
        }
        #endregion
    }
}
