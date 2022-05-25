export const state = () => ({
    URL: "https://localhost:44367/api/",
    token: "No Token",
})
export const mutations = {
    SetToken(state, { token }) {

        this.token = token;

    }
}