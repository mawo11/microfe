export type  MenuItem  = {
    url: string
    name: string
}

export type MenuResponse = {
    items: MenuItem[]
}

export type UserLoginResponse = {    
    success: boolean,
    urlToRedirect: string,
}