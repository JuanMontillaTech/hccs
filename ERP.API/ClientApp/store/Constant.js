export const state = () => ({
    counter: 0 ,
    rootApi: "https://localhost:44367/" 
  }) 

  export const mutations = {
    increment(state) {
      state.counter++
    }
  }
  