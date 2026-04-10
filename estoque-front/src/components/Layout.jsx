import { Package, Plus, Boxes } from "lucide-react";
import { NavLink } from "react-router-dom";

function Layout({ children }) {
  const base =
    "flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium transition";
  const inactive = "text-slate-300 hover:bg-slate-800 hover:text-white";
  const active =
    "bg-emerald-500/15 text-emerald-400 border border-emerald-500/20";

  return (
    <div className="flex min-h-screen bg-slate-100">
      {/* SIDEBAR */}
      <aside className="w-72 bg-slate-900 text-white px-6 py-8">
        <div className="mb-10">
          <div className="flex items-center gap-3">
            <div className="h-11 w-11 flex items-center justify-center rounded-xl bg-emerald-500/20">
              <Boxes className="text-emerald-400" />
            </div>
            <div>
              <h1 className="text-xl font-bold">Estoque</h1>
              <p className="text-sm text-slate-400">Admin</p>
            </div>
          </div>
        </div>

        <nav className="flex flex-col gap-2">
          <NavLink
            to="/"
            className={({ isActive }) =>
              `${base} ${isActive ? active : inactive}`
            }
          >
            <Package size={16} />
            Produtos
          </NavLink>

          <NavLink
            to="/create"
            className={({ isActive }) =>
              `${base} ${isActive ? active : inactive}`
            }
          >
            <Plus size={16} />
            Novo Produto
          </NavLink>
        </nav>
      </aside>

      {/* CONTEÚDO */}
      <main className="flex-1 p-8">{children}</main>
    </div>
  );
}

export default Layout;