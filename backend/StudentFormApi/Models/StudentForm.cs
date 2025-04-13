using System;

namespace StudentFormApi.Models
{
    public class StudentForm
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Facultate { get; set; }
        public string Motivatie { get; set; }
        public DateTime DataSubmisiei { get; set; }
    }
}
