import http from 'k6/http';
import {check} from 'k6';

export const options = {
    vus: 1,
    iterations: 1,
};

function generateRandomString(length) {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    for (let i = 0; i < length; i++) {
        result += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return result;
}

export default () => {
    const baseUrl = 'http://localhost:5093/controller';
    const subjects = [];

    for (let i = 0; i < 200; i++) {
        const subjectData = {
            Name: generateRandomString(20)
        };
        const response = http.post(`${baseUrl}/CreateSubject`, JSON.stringify(subjectData), {
            headers: {'Content-Type': 'application/json'}
        });
        check(response, {'Subject created': (r) => r.status === 200});
        if (response.status === 200) {
            const subjectId = JSON.parse(response.body);
            subjects.push(subjectId);
        }
    }

    const termsPerSubject = 5;
    for (let subjectIndex = 0; subjectIndex < subjects.length; subjectIndex++) {
        for (let termIndex = 0; termIndex < termsPerSubject; termIndex++) {
            const termData = {
                Name: generateRandomString(20),
                Definition: generateRandomString(200),
                SubjectId: subjects[subjectIndex]
            };
            const response = http.post(`${baseUrl}/CreateTerm`, JSON.stringify(termData), {
                headers: {'Content-Type': 'application/json'}
            });
            check(response, {'Term created': (r) => r.status === 200});
        }
    }
};