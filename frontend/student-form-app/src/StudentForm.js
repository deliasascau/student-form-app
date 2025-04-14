import React, { useState } from "react";
import "./StudentForm.css"; // importă fișierul de stil

function StudentForm() {
  const [formData, setFormData] = useState({
    nume: "",
    prenume: "",
    facultate: "",
    motivatie: ""
  });
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validare: motivația trebuie să aibă cel puțin 100 de caractere
    if (formData.motivatie.trim().length < 100) {
      alert("Motivația participării trebuie să aibă minim 100 de caractere.");
      return;
    }

    setLoading(true);
    setSuccess(false);

    try {
      const response = await fetch("http://localhost:5001/api/StudentForms", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData)
      });

      if (!response.ok) {
        throw new Error("Eroare la trimiterea formularului");
      }

      // Primim PDF-ul generat de backend
      const blob = await response.blob();

      // Creăm un link pentru a descărca fișierul PDF
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", "FisaStudent.pdf");
      document.body.appendChild(link);
      link.click();
      link.remove();

      setSuccess(true);
      // Resetăm formularul
      setFormData({ nume: "", prenume: "", facultate: "", motivatie: "" });
    } catch (err) {
      alert(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="page-container">
      <header className="header">
        <img src="/logo_unitbv.png" alt="Universitatea Transilvania" className="logo" />
        <h1>Formular Student</h1>
      </header>

      <div className="form-wrapper">
        <form onSubmit={handleSubmit} className="form-card">
          <label htmlFor="nume">Nume</label>
          <input
            type="text"
            name="nume"
            value={formData.nume}
            onChange={handleChange}
            required
            placeholder="Popescu"
          />

          <label htmlFor="prenume">Prenume</label>
          <input
            type="text"
            name="prenume"
            value={formData.prenume}
            onChange={handleChange}
            required
            placeholder="Ana"
          />

          <label htmlFor="facultate">Facultate</label>
          <input
            type="text"
            name="facultate"
            value={formData.facultate}
            onChange={handleChange}
            required
            placeholder="Inginerie Electrică"
          />

          <label htmlFor="motivatie">Motivație participare (minim 100 de caractere)</label>
          <textarea
            name="motivatie"
            value={formData.motivatie}
            onChange={handleChange}
            required
            placeholder="Spune-ne de ce vrei să participi..."
          />

          <button type="submit" disabled={loading}>
            {loading ? "Se trimite..." : "Trimite și descarcă PDF"}
          </button>

          {success && (
            <p className="success-message">
              PDF-ul a fost generat și descărcat cu succes!
            </p>
          )}
        </form>
      </div>
    </div>
  );
}

export default StudentForm;
