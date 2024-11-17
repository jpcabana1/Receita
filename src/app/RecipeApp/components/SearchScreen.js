import React, { useEffect, useContext, createContext, useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet, FlatList, ActivityIndicator, Modal, TouchableOpacity, Image } from 'react-native';
import { styles } from '../assets/styles/styles'
import { host } from '../util/variables';

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

export default SearchScreen;