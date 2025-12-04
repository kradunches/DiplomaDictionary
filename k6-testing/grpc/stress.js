import {check, sleep} from 'k6';
import {Client, StatusOK} from 'k6/net/grpc';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

export const options = {
    scenarios: {
        stress_test_grpc: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                {duration: '2m', target: 10},   // начальная лёгкая нагрузка
                {duration: '2m', target: 30},   // рост
                {duration: '3m', target: 50},   // умеренная нагрузка
                {duration: '3m', target: 80},   // выше средней
                {duration: '4m', target: 120},  // интенсивная
                {duration: '4m', target: 160},  // высокая
                {duration: '4m', target: 200},  // пиковая (стресс)
                {duration: '3m', target: 0},    // сброс нагрузки
            ],
            gracefulRampDown: '1m',
        },
    },
    thresholds: {
        // grpc_req_failed: ['rate<0.05'],
        grpc_req_duration: [
            'p(95)<1500',
            'p(99)<3000',
        ],
    },
};

const client = new Client();
client.load(['F:/CSharp/DiplomaDictionary/GrpcService/Protos'], 'concept.proto');

export default function () {
    client.connect('localhost:5281', {plaintext: true});

    const response = client.invoke('concept.SubjectService/GetAllTerms', {});

    check(response, {
        'status is OK': (r) => r && r.status === StatusOK,
    });

    client.close();
    sleep(1);
}

export function handleSummary(data) {
    return {
        'results/stress_getallterms_report.html': htmlReport(data),
    };
}
