import { Routes, Route } from "react-router-dom";
import "./App.css";
import ProductDetailsPage from "./pages/ProductDetailsPage.tsx";
import  Homepage from "./pages/Homepage.tsx";
import { Navbar } from "./nav/Navbar.tsx";
import ProductsPage from "./pages/ProductsPage.tsx"

export default function App() {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/" element={<Homepage />} />
        <Route path="/products">
          <Route index element={<ProductsPage />} />
          <Route path=":id" element={<ProductDetailsPage />} />
        </Route>
      </Routes>
    </>
  );
}