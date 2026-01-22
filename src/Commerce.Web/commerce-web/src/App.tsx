import { Routes, Route } from "react-router-dom";
import "./App.css";
import ProductDetailsPage from "./ProductDetailsPage.tsx";
import Homepage from "./Homepage.tsx";
import { Navbar } from "./Navbar";

export default function App() {
    const categories = [
    { label: "Caps", to: "/?cat=caps" },
    { label: "Tees", to: "/?cat=tees" },
    { label: "Hoodies", to: "/?cat=hoodies" },
    { label: "Pants", to: "/?cat=pants"}]
    const brands = [
    { label: "Nike", to: "/?brand=Nike" },
    { label: "Adidas", to: "/?brand=Adidas" },
    { label: "UnderArmor", to: "/?brand=UnderArmor" },
    { label: "Reebok", to: "/?brand=Reebok"}]
  return (
    <>
      <Navbar categories={categories} brands={brands}/>
      <Routes>
        <Route path="/" element={<Homepage />} />
        <Route path="/products/:id" element={<ProductDetailsPage />} />
      </Routes>
    </>
  );
}