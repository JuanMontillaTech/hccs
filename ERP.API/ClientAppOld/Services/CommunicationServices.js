import axios from 'axios';
export default class CommunicationServices {
    constructor(CommunicationEntity) {
        this.CommunicationEntity = CommunicationEntity;
    }
    async GetBody(data) {

        console.log(data);
        const result = await axios.get(
            `https://localhost:44367/api/Security/Login`,
            data
        );
        // await axios({
        //     method: 'Get',
        //     url: `https://localhost:44367/api/Security/Login`,
        //     headers: {},
        //     data: {
        //         data // This is the body part
        //     }
        // }).then(res => {
        //     result = res;
        // });
        return result.data;
    }

    async getAll() {
        const url = `/${this.CommunicationEntity}`;

        console.log(this.$axios);
        let result = '';
        await $axios.get(url).then(res => {
            result = res;
        });

        return result.data;
    }
    async get(id) {
        const result = await this.$axios.$get(
            this.CommunicationEntity + "/" + id
        );
        return result.data;
    }


    async getView(id) {
        let result = await this.$axios.$get(
            this.CommunicationEntity + "/View/" + id
        );

        return result.data;
    }



    async create(data) {
        const result = await this.$axios.$post(
            this.CommunicationEntity,
            data
        );
        return result.data;
    }

    async createData(data) {
        const result = this.$axios({
                method: "post",
                url: this.CommunicationEntity,
                data: data,
                headers: { "Content-Type": "multipart/form-data" }
            })
            .then(function(response) {
                //handle success
                console.log(response);
            })
            .catch(function(response) {
                //handle error
                console.log(response);
            });
        return result.data;
    }

    async update(data) {
        const result = await this.$axios.$put(
            this.CommunicationEntity,
            data
        );
        return result.data;
    }

    async delete(id) {
        const result = await this.$axios.$delete(
            this.CommunicationEntity + "/" + id
        );
        return result.data;
    }
}