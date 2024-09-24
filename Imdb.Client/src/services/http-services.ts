import axios from "axios";
import { environment } from "../environment/environment";

export class HttpServicesService {
  
    public static httpGet(endpoint: string){
      return axios.get(environment.baseUrl + endpoint);
    }
  
  }