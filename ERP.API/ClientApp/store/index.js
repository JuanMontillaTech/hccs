export const state = () => ({
    // URL: "https://localhost:44367/api/",
    URL: "http://54.221.85.23:8081/api/",
    token: "No Token",
})
export const mutations = {
    SetToken(state, { token }) {

        this.token = token;

    }
}