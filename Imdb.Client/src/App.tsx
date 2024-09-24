import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import { MovieModel, SearchResult } from './model/movie.model';
import MovieService from './services/movie-service';


function App() {

const[selectedMovie, setMovie] = useState<SearchResult>();
const[movies, setMovies] = useState<MovieModel[]>([]);
const[years, setYears] = useState<number[]>([]);
const[title, setTitle] = useState<string>("");
const[year, setMovieYear] = useState<number>();

const selectMovie = (imdbID: string | undefined)=>{
  let movie = movies.find(x=> x.searchResult.imdbID == imdbID)?.searchResult;
  setMovie(movie);
}

const searchMovie = ()=>{
  
  let query:Record<string, string> = {
    Plot: (document.getElementById('shortRadio') as HTMLInputElement).checked ? 'short' : 'full',
    Year: year == undefined ? '' : year.toString()
  }

  if(query['Year'] == '' || query['Year'].toString() == 'NaN') {
    delete query['Year'];
  }

  if (title != '') {
    MovieService.movieSearch(title, query).then(data => {
      let movie = data.data;
      setMovie(movie);
    }).finally(() => {
      MovieService.getSearchResults().then(data => {
        setMovies(data.data);
      });

    });
  }
}

 useEffect(() => {
  let yearsCount = [];
  for(let i = 2024; i >= 1900; i--){
    yearsCount.push(i); //yearsCount
  }
  setYears(yearsCount);
  (document.getElementById('shortRadio') as HTMLInputElement).checked = true;
  MovieService.getSearchResults().then(data=>{
    setMovies(data.data);
  });
 },[]);


  return (
    <div className="App">
      <header className="">
        <nav className="navbar navbar-expand-lg navbar-light">
          <div className="container">
              
              <a className="navbar-brand text-bold" href="#">Imdb Search</a>            
              <form className="d-flex mx-auto">
                  <input className="form-control me-2 w-75" type="search" onChange={(e)=>setTitle(e.target.value)} placeholder="Search" aria-label="Search"/>
                  <button className="btn btn-primary" onClick={searchMovie} type="button">Search</button>
              </form>
          </div>
        </nav>
      </header>
      <div className="vertical-center">
        <div className="container">
          <div className="row">
            <div className="col-lg-6 mt-4" >
              <div className="searchControl">
                <div className="row">
                  <div className="col-lg-6">
                    <label className='text-bold'>Plot</label>
                    <div className="row ">
                      <div className="col-lg-6">
                        <div className="form-check controlBody">
                          <input className="form-check-input" type="radio" name="radioPlot" id="shortRadio" />
                          <label className="form-check-label text-start" htmlFor="flexRadioDefault2">
                            Short
                          </label>
                        </div>
                      </div>
                      <div className="col-lg-6">
                        <div className="form-check controlBody">
                          <input className="form-check-input" type="radio" name="radioPlot" id="fullRadio" />
                          <label className="form-check-label" htmlFor="flexRadioDefault2">
                            Full
                          </label>
                        </div>
                      </div>
                    </div>

                  </div>
                  <div className="col-lg-6">
                    <label className='text-bold text-start'>Year</label>
                    <select className="form-select controlBody" onChange={(e)=>setMovieYear(parseInt(e.target.value))} aria-label="Default select example">
                      <option defaultValue={""}></option>
                      {years.map((year) => <option key={year} value={year}>{year}</option>)}
                    </select>
                  </div>
                </div>

              </div>
              <div className="container mt-4">
                {selectedMovie != undefined ? (
                <div className="row">
                  <div className="col-lg-5 text-start">
                    {selectedMovie?.poster ? 
                        <img width={'100%'} src={selectedMovie?.poster} /> :
                        <label >Image link not available</label>}
                  </div>
                  <div className="col-lg-7 bg-default text-start pl-5">
                    <span><label className='text-bold'>Title:</label> {selectedMovie?.title}</span><br />
                    <span><label className='text-bold'>Year: </label> {selectedMovie?.year }</span><br />
                    <span><label className='text-bold'>IMDB Score: </label> {selectedMovie?.imdbRating }</span><br />
                    <span><label className='text-bold'>Plot: </label> {selectedMovie?.plot}</span><br />
                  </div>
                </div>):(
                <div>
                  <p>No Movie found</p>
                </div>)
                }
              </div>
              
            </div>
            <div className="col-lg-6 mt-4">
              <p className='text-bold'>Search Results</p>
              <table className="table table-striped">
                <thead className="thead-light">
                  <tr>
                    <td className='text-start'>Title</td>

                    <td>View Details</td>
                  </tr>
                </thead>
                <tbody>
                  {movies?.map((movie) => (
                    <tr key={movie.searchResult.imdbID}>
                    <td className='text-start ms-5'>{movie.title}</td>
                    <td><a className="btn btn-outline-primary" onClick={() => selectMovie(movie.searchResult.imdbID)} >View Details</a>
                    </td>
                  </tr>
                ))}
                  
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
