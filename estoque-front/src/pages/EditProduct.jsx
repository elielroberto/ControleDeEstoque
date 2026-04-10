import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Layout from "../components/Layout";
import api from "../services/api";

function EditProduct() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    sku: "",
    name: "",
    description: "",
    categoryId: "",
    minStock: "",
  });

  useEffect(() => {
    loadProduct();
  }, []);

  async function loadProduct() {
    const res = await api.get(`/products/${id}`);
    const p = res.data;

    setForm({
      sku: p.sku,
      name: p.name,
      description: p.description,
      categoryId: p.categoryId,
      minStock: p.minStock,
    });
  }

  function handleChange(e) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();

    await api.put(`/products/${id}`, {
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
        <h2 className="text-2xl font-bold mb-6">Editar Produto</h2>

        <form
          onSubmit={handleSubmit}
          className="bg-white p-6 rounded-xl shadow space-y-4"
        >
          <input name="sku" value={form.sku} onChange={handleChange} className="w-full border p-2 rounded" />
          <input name="name" value={form.name} onChange={handleChange} className="w-full border p-2 rounded" />
          <textarea name="description" value={form.description} onChange={handleChange} className="w-full border p-2 rounded" />
          <input name="categoryId" value={form.categoryId} onChange={handleChange} className="w-full border p-2 rounded" />
          <input name="minStock" value={form.minStock} onChange={handleChange} className="w-full border p-2 rounded" />

          <button className="w-full bg-blue-600 text-white py-2 rounded">
            Atualizar
          </button>
        </form>
      </div>
    </Layout>
  );
}

export default EditProduct;