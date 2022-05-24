import axios from 'axios';
import * as names from '../names';

const Login = (data) => {
    let url = `${names.API_ENDPOINT_SECURITY}/Login`;
    return new Promise((resolve, reject) => {
        axios.post(url, data, {headers: {
            "Content-Type": "application/json"}
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error)
            })
    });
}

const getDataById = (Id) => {
    let url = `${names.API_ENDPOINT_SECURITY}${Id}`;
    return new Promise((resolve, reject) => {
        axios.get(url, {headers: {
            "Content-Type": "application/json"}
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            })
    })
}

const getInstitutiondataById = (Id) => {
    let url = `${names.API_ENDPOINT_SECURITY}/${Id}`;
    return new Promise((resolve, reject) => {
        axios.get(url, {headers: {
            "Content-Type": "application/json"}
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            })
    });
}

const postData = (data) => {
    let url = `${names.API_ENDPOINT_SECURITY}`;
    return new Promise((resolve, reject) => {
        axios.post(url, data, {headers: {
            "Content-Type": "application/json"}
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error)
            })
    });
}

const putData = (data) => {
    let url = `${names.API_ENDPOINT_SECURITY}`;
    return new Promise((resolve, reject) => {
        axios.put(url, data, {headers: {
            "Content-Type": "application/json"}
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            })
    });
}

const deleteData = (Id, userEmail) => {
    let url = `${names.API_ENDPOINT_SECURITY}/${Id}/${userEmail}`;
    return new Promise((resolve, reject) => {
        axios.delete(url, {headers: {
            "Content-Type": "application/json"}
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            })
    });
}

const getDataPagination = (params) => {
    let url = `${names.API_ENDPOINT_SECURITY}`;
    return new Promise((resolve, reject) => {
        axios.get(url, { params }, {headers: {
            "Content-Type": "application/json"}
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            })
    })
}



export {
    Login,
    postData,
    putData,
    deleteData,
    getDataPagination,
    getDataById
};