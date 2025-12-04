// javascript
import http from 'k6/http';
import {check, sleep} from 'k6';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

// резкий скачок и резкий спад
export const options = {
    scenarios: {
        spike_test: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                {duration: '30s', target: 5},   // лёгкий разогрев
                {duration: '30s', target: 50},  // резкий скачок (spike)
                {duration: '2m', target: 50},  // удержание пика
                {duration: '1m', target: 0},   // быстрый спад до 0
            ],
            gracefulRampDown: '30s',
        },
    },
    thresholds: {
        http_req_failed: ['rate<0.05'], // до 5% ошибок допускается в пике
        http_req_duration: [
            'p(95)<2000',
            'p(99)<4000',
        ],
    },
};

export default function () {
    const res = http.get('http://localhost:5093/controller/GetAllTerms');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1); // пауза между запросами одного VU
}

export function handleSummary(data) {
    return {
        'results/spike_getallterms_report.html': htmlReport(data),
    };
}
