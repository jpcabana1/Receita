import React, { useEffect, useContext, createContext, useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet, FlatList, ActivityIndicator, Modal, TouchableOpacity, Image } from 'react-native';

export const DataContext = createContext();
const host = "https://recipe-jpcabana-bpdjekggbgbrb8cy.brazilsouth-01.azurewebsites.net/"

const App = () => {
    return (
        <DataProvider>
            <View style={styles.container}>
                <AuthenticationHandlerComponent />
            </View>
        </DataProvider>
    );
};

const AuthenticationHandlerComponent = () => {
    const { token } = useContext(DataContext);

    return (
        token !== "" ? <SearchScreen /> : <LoginScreen />
    );
}

export const DataProvider = ({ children }) => {
    const [token, setToken] = useState("");

    return (
        <DataContext.Provider value={{ token, setToken }}>
            {children}
        </DataContext.Provider>
    );
};

const LoginScreen = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const { setToken } = useContext(DataContext);

    const handleLogin = () => {
        fetch(`${host}/login`, {
            method: "POST",
            headers: {
                "accept": "*/*",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "username": username,
                "password": password
            }),
            redirect: "follow"
        })
            .then(res => res.json())
            .then(result => setToken(result.token))
            .catch(error => alert(`Credenciais inválidas`))

    };

    return (
        <View style={styles.container}>
            <Text style={styles.title}>Login</Text>
            <TextInput
                style={styles.input}
                placeholder="Usuário"
                value={username}
                onChangeText={setUsername}
            />
            <TextInput
                style={styles.input}
                placeholder="Senha"
                secureTextEntry
                value={password}
                onChangeText={setPassword}
            />
            <Button title="Entrar" onPress={handleLogin} />
        </View>
    );
};

const SearchScreen = () => {
    const [searchText, setSearchText] = useState('');
    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(false);
    const [modalVisible, setModalVisible] = useState(false);
    const [selectedItem, setSelectedItem] = useState(null);
    const { token, } = useContext(DataContext);
    const { setToken, } = useContext(DataContext);

    const handleSearch = async () => {
        setLoading(true);

        const myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        myHeaders.append("Authorization", `Bearer ${token}`);

        const raw = JSON.stringify({
            "search": searchText == '' ? "*" : searchText,
            "select": "",
            "filter": "",
            "count": true,
            "top": 10,
            "skip": 0
        });

        const requestOptions = {
            method: "POST",
            headers: myHeaders,
            body: raw,
            redirect: "follow"
        };
        console.log(token)
        fetch(host + "/Search", requestOptions)
            .then((response) => response.json())
            .then((result) => {
                console.log(result)
                setResults(result)
            })
            .catch((error) => console.error('Erro ao buscar dados:', error));

        setLoading(false);

    };

    const openModal = (item) => {
        setSelectedItem(item);
        setModalVisible(true);
    };

    const closeModal = () => {
        setModalVisible(false);
        setSelectedItem(null);
    };

    useEffect(() => {
        handleSearch()
        return () => {
            console.log("Component unmounted!");
        }
    }
        , []);

    return (
        <View style={styles.container}>
            <Text style={styles.title}>Busca</Text>
            <TextInput
                style={styles.searchInput}
                placeholder="Digite sua pesquisa..."
                value={searchText}
                onChangeText={(text) => setSearchText(text)}
            />

            <View style={styles.buttonRow}>
                <Button title="Buscar" onPress={handleSearch} />
                <Button color="red" title="Sair" onPress={() => setToken("")} />
            </View>

            {loading ? (
                <ActivityIndicator size="large" color="#0000ff" style={styles.loading} />
            ) : (
                <FlatList
                    data={results}
                    keyExtractor={(item, index) => index.toString()}
                    renderItem={({ item }) => (
                        <TouchableOpacity onPress={() => openModal(item)}>
                            <View style={styles.resultItem}>
                                <Image source={{ uri: item.thumbnail }} style={styles.thumbnail} />
                                <Text style={styles.resultTitle}>{item.nome}</Text>
                            </View>
                        </TouchableOpacity>
                    )}
                    style={styles.resultsList}
                />
            )}

            {selectedItem && (
                <Modal
                    visible={modalVisible}
                    transparent={true}
                    animationType="slide"
                    onRequestClose={closeModal}
                >
                    <View style={styles.modalOverlay}>
                        <View style={styles.modalContent}>
                            <Text style={styles.modalTitle}>{selectedItem.nome}</Text>
                            <Text style={styles.modalDescription}>{selectedItem.descricao}</Text>

                            <Text style={styles.modalTitle}>Ingredientes:</Text>
                            {selectedItem.ingredientes.map((item, index) => (
                                <View key={index} style={styles.listItem}>
                                    <Text style={styles.bullet}>•</Text>
                                    <Text style={styles.instruction}>{item}</Text>
                                </View>
                            ))}

                            <Text style={styles.modalTitle}>Modo de Preparo:</Text>
                            {selectedItem.modo_preparo.map((item, index) => (
                                <View key={index} style={styles.listItem}>
                                    <Text style={styles.bullet}>•</Text>
                                    <Text style={styles.instruction}>{item}</Text>
                                </View>
                            ))}


                            <Button title="Fechar" onPress={closeModal} />
                        </View>
                    </View>
                </Modal>
            )}
        </View>
    );
};



const styles = StyleSheet.create({
    buttonRow: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        width: '100%',
        marginBottom: 20,
    },
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#fff',
        paddingHorizontal: 20,
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        marginBottom: 20,
    },
    input: {
        height: 40,
        width: '100%',
        borderColor: '#ddd',
        borderWidth: 1,
        borderRadius: 10,
        paddingHorizontal: 15,
        marginBottom: 15,
    },
    searchInput: {
        height: 40,
        width: '100%',
        borderColor: '#ddd',
        borderWidth: 1,
        borderRadius: 20,
        paddingHorizontal: 15,
        marginBottom: 20,
    },
    loading: {
        marginTop: 20,
    },
    resultsList: {
        marginTop: 20,
        width: '100%',
    },
    resultItem: {
        padding: 15,
        borderBottomWidth: 1,
        borderBottomColor: '#ddd',
    },
    thumbnail: {
        width: 50,
        height: 50,
        borderRadius: 5,
        marginRight: 10,
    },
    resultTitle: {
        fontSize: 16,
        fontWeight: 'bold',
    },
    resultDescription: {
        fontSize: 14,
        color: '#666',
    },
    modalOverlay: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
    },
    modalContent: {
        width: '80%',
        backgroundColor: '#fff',
        padding: 20,
        borderRadius: 10,
        alignItems: 'left',
    },
    modalTitle: {
        fontSize: 18,
        fontWeight: 'bold',
        marginBottom: 10,
    },
    modalDescription: {
        fontSize: 16,
        color: '#666',
        marginBottom: 20,
    },
    listItem: {
        flexDirection: 'row',
        alignItems: 'center',
        marginBottom: 5,
    },
    bullet: {
        fontSize: 18,
        marginRight: 10,
    },
    instruction: {
        fontSize: 16,
        color: '#333',
    },
    tipo: {
        fontSize: 15,
        fontWeight: 'bold',
        color: '#d6b015',
    },
});

export default App;
