export const state = () => ({
    counter: 0 ,
    rootApi: "https://localhost:44367/",
    token: null
  }) 

  export const mutations = {
    increment(state) {
      state.counter++
    },
    setToken(state) {
      state.token = localStorage.getItem("token");
    }
  }

  export const actions = {
    getToken(){
      commit('setToken')
    }
  }
  