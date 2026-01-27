import { Routes, Route } from "react-router-dom";
import "./App.css";
import ProductDetailsPage from "./ProductDetailsPage.tsx";
import  Homepage from "./Homepage.tsx";
import { Navbar } from "./Navbar";
import ProductsPage from "./ProductsPage"

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