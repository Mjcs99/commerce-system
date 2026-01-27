import type { Product } from "../../types/Product";

export type GetProductsResponse = {
  items: Product[];
};

export type GetCategorySlugsResponse = {
  categorySlugs: string[];
}