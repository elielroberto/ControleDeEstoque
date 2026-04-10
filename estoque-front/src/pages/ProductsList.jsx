import { useEffect, useState } from "react";
import { Pencil, Trash2, Plus } from "lucide-react";
import api from "../services/api";
import { useNavigate } from "react-router-dom";
import Layout from "../components/Layout";

function ProductsList() {
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    loadProducts();
  }, []);

  async function loadProducts() {
    const res = await api.get("/products");
    setProducts(res.data);
  }

  async function handleDelete(id) {
    if (!confirm("Excluir produto?")) return;

    await api.delete(`/products/${id}`);
    setProducts((prev) => prev.filter((p) => p.id !== id));
  }

  return (
    <Layout>
      <div className="max-w-7xl mx-auto">
        {/* HEADER */}
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-3xl font-bold">Produtos</h2>

          <button
            onClick={() => navigate("/create")}
            className="bg-emerald-600 text-white px-4 py-2 rounded-lg flex items-center gap-2"
          >
            <Plus size={16} />
            Novo
          </button>
        </div>

        {/* TABELA */}
        <div className="bg-white rounded-xl shadow overflow-hidden">
          <table className="w-full">
            <thead className="bg-slate-50 text-sm text-slate-500 uppercase">
              <tr>
                <th className="p-4 text-left">ID</th>
                <th className="p-4 text-left">SKU</th>
                <th className="p-4 text-left">Nome</th>
                <th className="p-4 text-left">Descrição</th>
                <th className="p-4 text-left">Categoria</th>
                <th className="p-4 text-left">Estoque</th>
                <th className="p-4 text-left">Status</th>
                <th className="p-4 text-right">Ações</th>
                <th className="p-4">Criado em</th>
                <th className="p-4">Atualizado</th>
              </tr>
            </thead>

            <tbody>
              {products.map((p) => (
                <tr key={p.id} className="border-t hover:bg-gray-50">
                  <td className="p-4">{p.id}</td>
                  <td className="p-4">{p.sku}</td>
                  <td className="p-4 font-semibold">{p.name}</td>
                  <td className="p-4 text-sm text-gray-500">{p.description}</td>
                  <td className="p-4">{p.categoryId}</td>
                  <td className="p-4">{p.minStock}</td>

                  <td className="p-4">
                    <span
                      className={`px-2 py-1 text-xs rounded ${
                        p.isActive
                          ? "bg-green-100 text-green-700"
                          : "bg-red-100 text-red-700"
                      }`}
                    >
                      {p.isActive ? "Ativo" : "Inativo"}
                    </span>
                  </td>

                  <td className="p-4 flex justify-end gap-2">
                    <button
                      onClick={() => navigate(`/edit/${p.id}`)}
                      className="bg-yellow-500 text-white px-3 py-1 rounded flex items-center gap-1"
                    >
                      <Pencil size={14} /> Editar
                    </button>

                    <button
                      onClick={() => handleDelete(p.id)}
                      className="bg-red-500 text-white px-3 py-1 rounded flex items-center gap-1"
                    >
                      <Trash2 size={14} /> Excluir
                    </button>
                  </td>
                  <td className="p-4">
                  {new Date(p.createdAt).toLocaleDateString()}
                </td>

                <td className="p-4">
                  {p.updatedAt 
                    ? new Date(p.updatedAt).toLocaleDateString() 
                    : "-"
                  }
                </td>
                </tr>
                
              ))}


            </tbody>
          </table>
        </div>
      </div>
    </Layout>
  );
}

export default ProductsList;