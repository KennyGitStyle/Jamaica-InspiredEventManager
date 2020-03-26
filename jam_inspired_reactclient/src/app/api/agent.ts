import axios, { AxiosResponse } from 'axios';
import {IEventActivity} from '../models/event-activity'
import { history } from '../..';
import { toast } from 'react-toastify';
import { IUser, IUserFormValues } from '../models/user';

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.request.use((config) => {
    const token = window.localStorage.getItem('jwt')
    if(token) config.headers.Authorization =`Bearer ${token}`
    return config
}, err => {
    return Promise.reject(err)
})

axios.interceptors.response.use(undefined, error => {
    if(error.message === 'Network Error' && !error.response){
        toast.error('Network connection error - check api connection!');
    }
    const {status, data, config} = error.response;
    if(status === 404) {
        history.push('/notfound');
    }
    if(status === 400 && config.method === 'get' && data.errors.hasOwnProperty('id')){
        history.push('/notfound');
    }
    if(status === 500){
        toast.error('Server error - check commandline for more info!');
    }
    
    throw error.response;
});

const responseBody = (response: AxiosResponse) => response.data;

const sleep = (ms: number) => (Response: AxiosResponse) => 
    new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(Response), ms))

const requests = {
    get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(sleep(1000)).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(sleep(1000)).then(responseBody),
    delete: (url: string) => axios.delete(url).then(sleep(1000)).then(responseBody)
}

const EventActivities = {
    list: (): Promise<IEventActivity[]> => requests.get('/activities'),
    details: (id: string) => requests.get(`/activities/${id}`),
    create: (activity: IEventActivity) => requests.post('/activities', activity),
    update: (activity: IEventActivity) => requests.put(`/activities/${activity.id}`, activity),
    delete: (id: string) => requests.delete(`/activities/${id}`),
    attend: (id: string) => requests.post(`/activities/${id}/attend`, {}),
    unattend: (id: string) => requests.delete(`/activities/${id}/attend`)

}

const User = {
    current: (): Promise<IUser> => requests.get('/user'),
    login: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/login`, user),
    register: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/register`, user)
}

export default  {
    EventActivities,
    User
}