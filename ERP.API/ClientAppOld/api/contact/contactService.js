import axios from "axios";
import * as names from "../names";

const getContacts = () => {
    let url = `${names.API_ENDPOINT_CONTACT}/GetAll`;
    return new Promise((resolve, reject) => {
        axios
            .get(url, {
                headers: {
                    "Content-Type": "application/json",
                },
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            });
    });
};

const getContactById = (Id) => {
    let url = `${names.API_ENDPOINT_CONTACT}${Id}`;
    return new Promise((resolve, reject) => {
        axios
            .get(url, {
                headers: {
                    "Content-Type": "application/json",
                },
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            });
    });
};

const postContact = (Contact) => {
    let url = `${names.API_ENDPOINT_CONTACT}`;
    return new Promise((resolve, reject) => {
        axios
            .post(url, Contact, {
                headers: {
                    "Content-Type": "application/json",
                },
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            });
    });
};

const putContact = (Contact) => {
    let url = `${names.API_ENDPOINT_CONTACT}`;
    return new Promise((resolve, reject) => {
        axios
            .put(url, Contact, {
                headers: {
                    "Content-Type": "application/json",
                },
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            });
    });
};

const deleteContact = (Id) => {
    let url = `${names.API_ENDPOINT_CONTACT}/${Id}`;
    return new Promise((resolve, reject) => {
        axios
            .delete(url, {
                headers: {
                    "Content-Type": "application/json",
                },
            })
            .then((response) => {
                resolve(response);
            })
            .catch((error) => {
                reject(error);
            });
    });
};

export { getContacts, getContactById, postContact, putContact, deleteContact };