import { get } from "../../shared/httpClient";
import type { GetProductsResponse, GetCategorySlugsResponse } from "./Responses";

export function getProducts(sp?: URLSearchParams) {
  const qs = sp?.toString();
  return get<GetProductsResponse>(qs ? `/api/v1/products?${qs}` : `/api/v1/products`);
}

export function getCategorySlugs(){
    return get<GetCategorySlugsResponse>("/api/v1/categories")
}