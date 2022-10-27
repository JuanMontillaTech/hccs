export const state = () => ({
   URL: "https://localhost:44367/api/",
   //URL: "https://api.administracionhccs.com/api/",
    token: "No Token",
})
export const mutations = {
    SetToken(state, { token }) {

        this.token = token;

    }
}