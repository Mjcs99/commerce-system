import type { ProductSummary } from "../../types/ProductSummary";

export type GetProductsResponse = {
  items: ProductSummary[];
};

export type GetProductDetailsResponse = {
  productId: string;
  name: string;
  price: Float32Array;
  description: string;
  images: string[];
};

export type GetCategorySlugsResponse = {
  categorySlugs: string[];
}