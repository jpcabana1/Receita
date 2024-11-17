export async function login(username, password) {
    const myHeaders = new Headers();
    myHeaders.append("accept", "*/*");
    myHeaders.append("Content-Type", "application/json");
    
    
    const raw = JSON.stringify({
        "username": username,
        "password": password
    });

    const requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
    };
    try {
        const response = await fetch("http://localhost:5273/login", requestOptions);
        console.log(result);
        return await response.json();
    } catch (error) {
        console.error(error);
        return {}
    }
}