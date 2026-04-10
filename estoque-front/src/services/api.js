import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7294/api"
});

export default api;