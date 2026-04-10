import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Layout from "../components/Layout";
import api from "../services/api";

function CreateProduct() {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    sku: "",
    name: "",
    description: "",
    categoryId: "",
    minStock: "",
  });

  function handleChange(e) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();

    await api.post("/products", {
      Sku: form.sku,
      Name: form.name,
      Description: form.description,
      CategoryId: Number(form.categoryId),
      MinStock: Number(form.minStock),
    });

    navigate("/");
  }

  return (
    <Layout>
      <div className="max-w-md mx-auto">
        <h2 className="text-2xl font-bold mb-6">Novo Produto</h2>

        <form
          onSubmit={handleSubmit}
          className="bg-white p-6 rounded-xl shadow space-y-4"
        >
          <input name="sku" placeholder="SKU" onChange={handleChange} className="w-full border p-2 rounded" />
          <input name="name" placeholder="Nome" onChange={handleChange} className="w-full border p-2 rounded" />
          <textarea name="description" placeholder="Descrição" onChange={handleChange} className="w-full border p-2 rounded" />
          <input name="categoryId" type="number" placeholder="Categoria" onChange={handleChange} className="w-full border p-2 rounded" />
          <input name="minStock" type="number" placeholder="Estoque mínimo" onChange={handleChange} className="w-full border p-2 rounded" />

          <button className="w-full bg-emerald-600 text-white py-2 rounded">
            Salvar
          </button>
        </form>
      </div>
    </Layout>
  );
}

export default CreateProduct;