import { HttpServicesService } from "./http-services";

export default class MovieService {
    static getSearchResults(){
        return HttpServicesService.httpGet('v1/movies/results');
      }
    
      static movieSearch(title: string, query: Record<string, string>){
        let queryString = new URLSearchParams(query).toString()
        return HttpServicesService.httpGet(`v1/movies/${title}?${queryString}`);
      }
}