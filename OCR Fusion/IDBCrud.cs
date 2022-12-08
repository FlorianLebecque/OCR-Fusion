namespace OCR_Fusion {
    public interface IDBCrud {

        public void Insert<T>(string table, T value);
        public void Update<T>(string table, T value);
        public List<T> Gets<T>(string table,string session);
        public void Delete<T>(string table,string session);

    }
}
